//Color Picker Widget (YUI based): By Dynamic Drive, at http://www.dynamicdrive.com
//Created: May 9th, 08'

//**Updated May 15th, 08': Script now auto colors preview controls based on hex value (if any) in corresponding input field when page loads.

//Requires:
//** YUI Color Picker: http://developer.yahoo.com/yui/colorpicker/ AND
//** DHTML Window Widget: http://www.dynamicdrive.com/dynamicindex8/dhtmlwindow/
// 2009-01-11  made change to set YUI color picker properties from config, so we canpass in localized labels

var ddcolorpicker = {

    definepicker: function(config) {
        var colorpicker = new YAHOO.widget.ColorPicker(config.colorcontainer[1], {
            ids: config.ids,
            txt: config.txt,
            showcontrols: true,
            showrgbcontrols: config.showrgbcontrols,
            showwebsafe: config.showwebsafe,
            showhsvcontrols: config.showhsvcontrols,
            showhexcontrols: config.showhexcontrols,
            showhexsummary: config.showhexsummary,
            images: config.images
        })
        colorpicker.on("rgbChange", function(colorobj) { //action to perform when the user selects a color within the colorpicker
            //colorobj.newValue: (array of R, G, B values),
            var colorhex = YAHOO.util.Color.rgb2hex(colorobj.newValue) //convert selected color value from rgb to hex
            var inputfield = window[config.colorcontainer[0] + '-active'] //reference currently active color input field
            var control = inputfield.relatedcontrol
            if (inputfield != null) { //if focus is currently on a color input field
                inputfield.value = "#" + colorhex //set input field value to the selected value inside color picker
                if (control != null && control.ispreview) { //if this input field contains a control and is previewed enabled (doesn't contain CSS class "nopreview")
                    control.style.backgroundColor = "#" + inputfield.value.replace(/^#/, '') //colorize control
                }
            }
        })

        // pre-select the color if its populated as hex on the input
        for (var i = 0; i < config.colorfields.length; i++) {
            //if control exists and the value inside field is a hex value (loose check)
            if (config.colorfields[i].control && config.colorfields[i].control.ispreview && this.validhexcheck(config.colorfields[i].input.value)) {
                colorpicker.setValue(YAHOO.util.Color.hex2rgb(config.colorfields[i].input.value.replace(/^#/, '')), true);
            }
        }
        
    },

    css: function(el, targetclass, action) {
        var needle = new RegExp("(^|\\s+)" + targetclass + "($|\\s+)", "ig")
        if (action == "check")
            return needle.test(el.className)
        else if (action == "remove")
            el.className = el.className.replace(needle, "")
        else if (action == "add")
            el.className += " " + targetclass
    },

    validhexcheck: function(value) { //loose valid hex check
        return /^#?\w{6}$/.test(value)
    },

    positionfloat: function(e, control, config) { //position floating window when a control is clicked on
        var e = window.event || e
        dhtmlwindow.getviewpoint()
        var t = window[config.colorcontainer[0] + '-float'] //reference DHTML window
        var leftpos = e.clientX + (control.offsetWidth > 50 ? 0 : control.offsetWidth)
        var toppos = e.clientY - control.offsetHeight
        if (toppos + t.offsetHeight - 20 > dhtmlwindow.docheight) { //DHTML window partially hidden towards bottom?
            toppos = (toppos - t.offsetHeight < 0) ? 10 : toppos - t.offsetHeight //(not enough room to flip upwards either?) 10px : flip upwards
        }
        t.moveTo(leftpos, toppos)
    },

    prefillpreview: function(config) {
        for (var i = 0; i < config.colorfields.length; i++) {
            if (config.colorfields[i].control && config.colorfields[i].control.ispreview && this.validhexcheck(config.colorfields[i].input.value)) //if control exists and the value inside field is a hex value (loose check)
                config.colorfields[i].control.style.backgroundColor = "#" + config.colorfields[i].input.value.replace(/^#/, '')
        }
    },
    
    init: function(config) {
        if (config.displaymode == "float") {
            //window[config.colorcontainer[0]+'-float'] is a global variable that references this floating DHTML Window:
            var t = window[config.colorcontainer[0] + '-float'] = dhtmlwindow.open(config.colorcontainer[0] + "box", "div", config.colorcontainer[0], config.floatattributes[0], config.floatattributes[1] + ",isColorPicker=1")
            t.style.visibility = "hidden"
            t.onclose = function() { this.hide(); return false } //when "x" icon is clicked in DHTML window, just hide instead of close window
            t.close = function() { dhtmlwindow.hide(this) } //OVERWRITE default t.close() function to just hide DHTML window
            t = null //Discard shortcut DHTML window var t
        }
        config.colorfields = []
        for (var i = 0; i < config.fields.length; i++) {
            var fieldbits = config.fields[i].split(':')
            config.colorfields.push({ input: document.getElementById(fieldbits[0]), control: document.getElementById(fieldbits[1]) })
            if (typeof fieldbits[1] != "undefined") {
                config.colorfields[i].input.relatedcontrol = document.getElementById(fieldbits[1])
                config.colorfields[i].control.relatedinput = document.getElementById(fieldbits[0])
                config.colorfields[i].control.ispreview = !ddcolorpicker.css(config.colorfields[i].control, 'nopreview', 'check') //is control preview enabled?
                config.colorfields[i].control.onclick = function(e) { //Behavior when control is clicked on
                    if (config.displaymode == "float") { //If "floating" color picker
                        window[config.colorcontainer[0] + '-float'].style.visibility = "visible" //show DHTML Window that's hidden when page first loads
                        window[config.colorcontainer[0] + '-float'].show() //show DHTML Window
                        ddcolorpicker.positionfloat(e, this, config)
                    }
                    this.relatedinput.focus()
                    return false //cancel default action of control (ie: if a link, cancel its navigation)
                } //end control.onclick
            }
            config.colorfields[i].input.onfocus = function() { //Behavior when focus is set on color input field
                window[config.colorcontainer[0] + '-active'] = this //make this field the currently active one (color picker writes to this field)
                if (!this.relatedcontrol && config.displaymode == "float") { //if this field DOESN'T have a control and display mode is "float"
                    window[config.colorcontainer[0] + '-float'].style.visibility = "visible" //show floating window onFocus of field
                    window[config.colorcontainer[0] + '-float'].show()
                    window[config.colorcontainer[0] + '-float'].moveTo("middle", "middle")
                }
            }
            config.colorfields[i].input.onblur = function() {
                if (this.relatedcontrol && this.relatedcontrol.ispreview && ddcolorpicker.validhexcheck(this.value)) //if this color field contains a control and the value inside field is a hex value (loose check)
                    this.relatedcontrol.style.backgroundColor = "#" + this.value.replace(/^#/, '')
            }
        }
        this.definepicker(config)
        dhtmlwindow.addEvent(window, function() { ddcolorpicker.prefillpreview(config) }, "load")
        dhtmlwindow.addEvent(window, function() { ddcolorpicker.uninit(config) }, "unload")
        ddcolorpicker.prefillpreview(config);


    },

    uninit: function(config) {
        for (var i = 0; i < config.colorfields.length; i++) {
            config.colorfields[i].input.onfocus = config.colorfields[i].input.onblur = null
            if (config.colorfields[i].control) {
                config.colorfields[i].control.onclick = config.colorfields[i].control.relatedinput = null
            }
            config.colorfields[i].input.relatedcontrol = null
            config.colorfields[i].input = config.colorfields[i].control = null
            config.colorfields[i] = null
        }
        config = null
    }

}