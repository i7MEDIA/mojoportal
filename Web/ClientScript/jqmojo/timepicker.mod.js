/*!
 * jQuery UI Timepicker 0.2.1
 *
 * Copyright (c) 2009 Martin Milesich (http://milesich.com/)
 *
 * Some parts are
 *   Copyright (c) 2009 AUTHORS.txt (http://jqueryui.com/about)
 *
 * $Id: timepicker.js 28 2009-08-11 20:31:23Z majlo $
 *
 * Depends:
 *  ui.core.js
 *  ui.datepicker.js
 *  ui.slider.js

 Extensive modifications by 
 Last Modified : 2010-06-27
 */
(function ($) {

    /**
    * Extending default values
    */
    $.extend($.datepicker._defaults, {
        'time24h': false, // True if 24h time
        'showTime': false, // Show timepicker with datepicker
        'altTimeField': '', // Selector for an alternate field to store time into
        'doneLabel': 'Finished'
    });

    /**
    * _hideDatepicker must be called with null
    */
    $.datepicker._connectDatepickerOverride = $.datepicker._connectDatepicker;
    $.datepicker._connectDatepicker = function (target, inst) {
        $.datepicker._connectDatepickerOverride(target, inst);

        // showButtonPanel is required with timepicker
        //if (this._get(inst, 'showTime')) {
        //inst.settings['showButtonPanel'] = true;
        //}

        var showOn = this._get(inst, 'showOn');

        if (showOn == 'button' || showOn == 'both') {
            // Unbind all click events
            inst.trigger.unbind('click');

            // Bind new click event
            inst.trigger.click(function () {
                if ($.datepicker._datepickerShowing && $.datepicker._lastInput == target)
                    $.datepicker._hideDatepicker(null); // This override is all about the "null"
                else
                    $.datepicker._showDatepicker(target);

                return false;
            });
        }
    };

    /**
    * Datepicker does not have an onShow event so I need to create it.
    * What I actually doing here is copying original _showDatepicker
    * method to _showDatepickerOverload method.
    */
    $.datepicker._showDatepickerOverride = $.datepicker._showDatepicker;
    $.datepicker._showDatepicker = function (input) {
        // Call the original method which will show the datepicker
        $.datepicker._showDatepickerOverride(input);

        input = input.target || input;

        // find from button/image trigger
        if (input.nodeName.toLowerCase() != 'input') input = $('input', input.parentNode)[0];

        // Do not show timepicker if datepicker is disabled
        if ($.datepicker._isDisabledDatepicker(input)) return;

        // Get instance to datepicker
        var inst = $.datepicker._getInst(input);

        var showTime = $.datepicker._get(inst, 'showTime');

        //var doneLabel = $.datepicker._get(inst, 'doneLabel');

        // If showTime = True show the timepicker
        if (showTime) $.timepicker.show(input);


    };

    /**
    * Same as above. Here I need to extend the _checkExternalClick method
    * because I don't want to close the datepicker when the sliders get focus.
    */
    $.datepicker._checkExternalClickOverride = $.datepicker._checkExternalClick;
    $.datepicker._checkExternalClick = function (event) {
        if (!$.datepicker._curInst) return;
        var $target = $(event.target);

        if (($target.parents("#" + $.timepicker._mainDivId).length == 0)) {
            $.datepicker._checkExternalClickOverride(event);
        }
    };

    /**
    * Datepicker has onHide event but I just want to make it simple for you
    * so I hide the timepicker when datepicker hides.
    */
    $.datepicker._hideDatepickerOverride = $.datepicker._hideDatepicker;
    $.datepicker._hideDatepicker = function (input, duration) {
        // Some lines from the original method
        var inst = this._curInst;

        if (!inst || (input && inst != $.data(input, PROP_NAME))) return;

        // Get the value of showTime property
        var showTime = this._get(inst, 'showTime');

        if (input === undefined && showTime) {
            if (inst.input) {
                inst.input.val(this._formatDate(inst));
                inst.input.trigger('change'); // fire the change event
            }

            this._updateAlternate(inst);

            if (showTime) $.timepicker.update(this._formatDate(inst));
        }

        // Hide datepicker
        $.datepicker._hideDatepickerOverride(input, duration);

        // Hide the timepicker if enabled
        if (showTime) {
            $.timepicker.hide();
        }
    };

    /**
    * This is a complete replacement of the _selectDate method.
    * If showed with timepicker do not close when date is selected.
    */
    $.datepicker._selectDate = function (id, dateStr) {
        var target = $(id);
        var inst = this._getInst(target[0]);
        var showTime = this._get(inst, 'showTime');
        dateStr = (dateStr != null ? dateStr : this._formatDate(inst));
        if (!showTime) {
            if (inst.input)
                inst.input.val(dateStr);
            this._updateAlternate(inst);
        }
        var onSelect = this._get(inst, 'onSelect');
        if (onSelect)
            onSelect.apply((inst.input ? inst.input[0] : null), [dateStr, inst]);  // trigger custom callback
        else if (inst.input && !showTime)
            inst.input.trigger('change'); // fire the change event
        if (inst.inline)
            this._updateDatepicker(inst);
        else if (!inst.stayOpen) {
            if (showTime) {
                this._updateDatepicker(inst);
            } else {
                this._hideDatepicker(null, this._get(inst, 'duration'));
                this._lastInput = inst.input[0];
                if (typeof (inst.input[0]) != 'object')
                    inst.input[0].focus(); // restore focus
                this._lastInput = null;
            }
        }
    };

    /**
    * We need to resize the timepicker when the datepicker has been changed.
    */
    $.datepicker._updateDatepickerOverride = $.datepicker._updateDatepicker;
    $.datepicker._updateDatepicker = function (inst) {
        $.datepicker._updateDatepickerOverride(inst);
        $.timepicker.resize();
    };

    function Timepicker() { }

    Timepicker.prototype = {
        init: function () {
            this._mainDivId = 'ui-timepicker-div';
            this._inputId = null;
            this._orgValue = null;
            this._orgHour = null;
            this._orgMinute = null;
            this._colonPos = -1;
            this._visible = false;
            this.tpDiv = $('<div id="' + this._mainDivId + '" class="ui-datepicker ui-widget ui-widget-content ui-helper-clearfix ui-corner-all ui-helper-hidden-accessible" style="width:25em; display: none; position: absolute;"></div>');
            this._generateHtml();

            $('#ui-datepicker-div').css('height', '25em');
        },

        show: function (input) {
            // Get instance to datepicker
            var inst = $.datepicker._getInst(input);

            this._time24h = $.datepicker._get(inst, 'time24h');
            this._altTimeField = $.datepicker._get(inst, 'altTimeField');

            this._inputId = input.id;

            if (!this._visible) {
                this._parseTime();
                this._orgValue = $("#" + this._inputId).val();
            }

            this.resize();

            $("#" + this._mainDivId).show();

            this._visible = true;

            var dpDiv = $("#" + $.datepicker._mainDivId);
            var dpDivPos = dpDiv.position();

            var viewWidth = (window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth) + $(document).scrollLeft();
            var tpRight = this.tpDiv.offset().left + this.tpDiv.outerWidth();

            if (tpRight > viewWidth) {
                dpDiv.css('left', dpDivPos.left - (tpRight - viewWidth) - 5);
                this.tpDiv.css('left', dpDiv.offset().left + dpDiv.outerWidth() + 'px');
            }


        },

        update: function (fd) {
            var curTime = $("#" + this._mainDivId + ' span.fragHours').text()
                    + ':'
                    + $("#" + this._mainDivId + ' span.fragMinutes').text();

            if (!this._time24h) {
                curTime += ' ' + $("#" + this._mainDivId + ' span.fragAmpm').text();
            }

            var curDate = $("#" + this._inputId).val();

            $("#" + this._inputId).val(fd + ' ' + curTime);

            if (this._altTimeField) {
                $(this._altTimeField).each(function () { $(this).val(curTime); });
            }
        },

        hide: function () {
            this._visible = false;
            $("#" + this._mainDivId).hide();
        },

        resize: function () {
            var dpDiv = $("#" + $.datepicker._mainDivId);
            var dpDivPos = dpDiv.position();

            var hdrHeight = $("#" + $.datepicker._mainDivId + ' > div.ui-datepicker-header:first-child').height();

            $("#" + this._mainDivId + ' > div.ui-datepicker-header:first-child').css('height', hdrHeight);

            this.tpDiv.css({
                'height': dpDiv.height(),
                'top': dpDivPos.top,
                'left': dpDivPos.left + dpDiv.outerWidth() - 1 + 'px'
            });

        },

        _generateHtml: function () {
            var doneLabel = "Done";
            try {
                if (jqtDoneLabel) { doneLabel = jqtDoneLabel; }
            } catch (err) { }

            var hourLabel = 'Hour';

            try {
                if (jqtHourLabel) { hourLabel = jqtHourLabel; }
            } catch (err) { }

            var minuteLabel = 'Minute';

            try {
                if (jqtMinuteLabel) { minuteLabel = jqtMinuteLabel; }
            } catch (err) { }

            var Hour24 = false;
            try {
                if (jqt24Hour) { Hour24 = jqt24Hour; }
            } catch (err) { }

            var amLabel = "AM";
            try {
                if (jqtAM) { amLabel = jqtAM; }
            } catch (err) { }

            var pmLabel = "PM";
            try {
                if (jqtPM) { pmLabel = jqtPM; }
            } catch (err) { }

            //alert('generate');
            var html = '';

            html += '<style type="text/css">';
            html += 'table.ui-timepicker a {text-align:center; margin:0px;} table.ui-timepicker td { padding:0px; } ';
            html += '.ui-datepicker table.ui-timepicker th {text-align:right;}';
            html += '.ui-datepicker table.ui-timepicker td {width:5%;}';
            html += '#ui-datepicker-div button.ui-datepicker-close {display:none;}';
            html += '</style>';

            html += '<div class="ui-datepicker-header ui-widget-header ui-helper-clearfix ui-corner-all">';
            html += '<div class="ui-datepicker-title" style="margin:0">';
            html += '<span class="fragHours">08</span><span class="delim">:</span><span class="fragMinutes">45</span> <span class="fragAmpm"></span></div></div>';
            html += '<table class="ui-timepicker"><tbody>';

            if (Hour24) {
                this._time24h = true;
                html += '<tr><th>' + hourLabel + '</th><td><a href="#">00</a></td><td><a href="#">01</a></td><td><a href="#">02</a></td><td><a href="#">03</a></td><td><a href="#">04</a></td><td><a href="#">05</a></td><td><a href="#">06</a></td><td><a href="#">07</a></td><td><a href="#">08</a></td><td><a href="#">09</a></td><td><a href="#">10</a></td><td><a href="#">11</a></td></tr>';
                html += '<tr><th></th><td><a href="#">12</a></td><td><a href="#">13</a></td><td><a href="#">14</a></td><td><a href="#">15</a></td><td><a href="#">16</a></td><td><a href="#">17</a></td><td><a href="#">18</a></td><td><a href="#">19</a></td><td><a href="#">20</a></td><td><a href="#">21</a></td><td><a href="#">22</a></td><td><a href="#">23</a></td></tr>';

            }
            else {
                html += '<tr><th>' + hourLabel + '</th><td><a href="#">12</a></td><td><a href="#">01</a></td><td><a href="#">02</a></td><td><a href="#">03</a></td><td><a href="#">04</a></td><td><a href="#">05</a></td><td><a href="#">06</a></td><td><a href="#">07</a></td><td><a href="#">08</a></td><td><a href="#">09</a></td><td><a href="#">10</a></td><td><a href="#">11</a></td></tr>';
            }

            html += '<tr><th>' + minuteLabel + '</th><td><a href="#">00</a></td><td><a href="#">05</a></td><td><a href="#">10</a></td><td><a href="#">15</a></td><td><a href="#">20</a></td><td><a href="#">25</a></td><td><a href="#">30</a></td><td><a href="#">35</a></td><td><a href="#">40</a></td><td><a href="#">45</a></td><td><a href="#">50</a></td><td><a href="#">55</a></td></tr>';
            html += '<tr><th>&nbsp;</th><td colspan="12"><div id="minuteSlider" class="slider"></div></td></tr>';

            if (!Hour24) {
                html += '<tr class="aprow"><th>&nbsp;</th><td><a id="ui-timepicker-am" href="#" type="hour">' + amLabel + '</a></td><td><a id="ui-timepicker-pm" href="#" type="hour">'
                + pmLabel + '</a></td><td>&nbsp;</td><td colspan="9">'
                + '<div >'
                + '<button class="ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all" onclick="$.datepicker._hideDatepicker();" type="button">' + doneLabel + '</button>'
                + '</div>'
                + '</td></tr>'
                + '</tbody></table>';
            }
            else {
                html += '<tr class="aprow"><th>&nbsp;</th><td></td><td>'
                + '</td><td>&nbsp;</td><td colspan="9">'
                + '<div >'
                + '<button class="ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all" onclick="$.datepicker._hideDatepicker();" type="button">' + doneLabel + '</button>'
                + '</div>'
                + '</td></tr>'
                + '</tbody></table>';

            }




            this.tpDiv.empty().append(html);
            $('body').append(this.tpDiv);

            $('#minuteSlider').slider({
                orientation: "horizontal",
                range: 'min',
                min: 0,
                max: 59,
                step: 1,
                slide: function (event, ui) {
                    self._setTime('minute', ui.value);
                },
                stop: function (event, ui) {
                    $('#' + self._inputId).focus();
                }
            });

            $('#minuteSlider > a').css('padding', 0);

            var prepare = function (selector, type) {
                $(selector).each(function () {

                    var e = $(this);
                    var time = parseInt(e.text().replace(/^0|:/, ''));
                    if (!this._time24h) {
                        if (time == 12) time = 0;
                    }

                    e.attr('type', type);
                    e.attr('time', time);

                    e.bind('click', function () {
                        var time = parseInt($(this).attr('time'));
                        var type = $(this).attr('type');
                        //alert($(this));
                        if (!this._time24h) {
                            if ((type == 'hour') && $('#ui-timepicker-pm').hasClass('ui-state-active')) time += 12;
                        }
                        self._setTime(type, time);
                        return false;
                    });
                });
            }

            $('table.ui-timepicker a').addClass('ui-state-default');
            if (!this._time24h) {
                //alert("12");
                prepare('table.ui-timepicker tr:nth-child(1) a', 'hour');
                prepare('table.ui-timepicker tr:nth-child(2) a', 'minute');
            }
            else {
                //alert("24");
                prepare('table.ui-timepicker tr:nth-child(1) a', 'hour');
                prepare('table.ui-timepicker tr:nth-child(2) a', 'hour');
                prepare('table.ui-timepicker tr:nth-child(3) a', 'minute');
            }

            $('#ui-timepicker-am').click(function () {
                self._setTime('hour', parseInt($("#" + self._mainDivId + ' span.fragHours').text()));
                return false;
            });

            $('#ui-timepicker-pm').click(function () {
                var hour = parseInt($("#" + self._mainDivId + ' span.fragHours').text());
                if (!this._time24h) {
                    if (hour >= 12) hour -= 12
                    self._setTime('hour', hour + 12);
                }
                else {
                    self._setTime('hour', hour);
                }
                return false;
            });

            $('#ui-datepicker-div button.ui-datepicker-close').click(function () {
                //alert('you rang');
                return false;
            });

            var self = this;
        },

        _writeTime: function (type, value) {
            if (type == 'hour') {
                if (!this._time24h) {

                    var amLabel = "AM";
                    try {
                        if (jqtAM) { amLabel = jqtAM; }
                    } catch (err) { }

                    var pmLabel = "PM";
                    try {
                        if (jqtPM) { pmLabel = jqtPM; }
                    } catch (err) { }

                    if (value < 12) {
                        $("#" + this._mainDivId + ' span.fragAmpm').text(amLabel);
                    } else {
                        $("#" + this._mainDivId + ' span.fragAmpm').text(pmLabel);
                        value -= 12;
                    }

                    if (value == 0) value = 12;
                } else {
                    $("#" + this._mainDivId + ' span.fragAmpm').text('');
                }

                if ((this._time24h) && (value < 10)) { value = '0' + value; }

                $("#" + this._mainDivId + ' span.fragHours').text(value);
            }

            if (type == 'minute') {
                if (value < 10) value = '0' + value;
                $("#" + this._mainDivId + ' span.fragMinutes').text(value);
            }
        },

        _parseTime: function () {
            var dt = $("#" + this._inputId).val();

            this._colonPos = dt.search(':');

            var amLabel = "AM";
            try {
                if (jqtAM) { amLabel = jqtAM; }
            } catch (err) { }

            var pmLabel = "PM";
            try {
                if (jqtPM) { pmLabel = jqtPM; }
            } catch (err) { }

            var m = 0, h = 0, a = '';

            if (this._colonPos != -1) {
                h = parseInt(dt.substr(this._colonPos - 2, 2), 10);
                m = parseInt(dt.substr(this._colonPos + 1, 2), 10);
                a = jQuery.trim(dt.substr(this._colonPos + 3, 3));
                if (a != amLabel && a != pmLabel) {
                    a = jQuery.trim(dt.substr(this._colonPos + 4, 3)); //this seems needed for Persian

                }
            }



            //a = a.toLowerCase();
            //alert(a);



            if (a != amLabel && a != pmLabel) {
                a = '';
            }

            if (h < 0) h = 0;
            if (m < 0) m = 0;

            if (h > 23) h = 23;
            if (m > 59) m = 59;

            if (!this._time24h) {
                if (a == pmLabel && h < 12) h += 12;
                if (a == amLabel && h == 12) h = 0;
            }

            this._setTime('hour', h);
            this._setTime('minute', m);

            this._orgHour = h;
            this._orgMinute = m;
        },

        _setTime: function (type, value) {
            if (isNaN(value)) value = 0;
            if (value < 0) value = 0;
            if (value > 23 && type == 'hour') value = 23;
            if (value > 59 && type == 'minute') value = 59;

            // set selected classes
            $('table.ui-timepicker td a[type=' + type + ']').removeClass('ui-state-active');

            var selected = value;

            if (type == 'hour') {
                if (!this._time24h) {
                    if (value < 12) {
                        //alert(value);
                        $('#ui-timepicker-am').addClass('ui-state-active');
                    } else {
                        $('#ui-timepicker-pm').addClass('ui-state-active');
                        selected -= 12;
                    }
                }

            }

            $('table.ui-timepicker td a[time=' + selected + '][type=' + type + ']').addClass('ui-state-active');

            if (type == 'minute') {
                $('#minuteSlider').slider('value', value);
            }

            this._writeTime(type, value);
        }
    };

    $.timepicker = new Timepicker();

    $('document').ready(function () { $.timepicker.init(); });

})(jQuery);