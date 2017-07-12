Type.registerNamespace('mojoAjaxControlToolkit');

// This is the base behavior for all extender behaviors
mojoAjaxControlToolkit.BehaviorBase = function(element) {
    /// <summary>
    /// Base behavior for all extender behaviors
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" domElement="true">
    /// Element the behavior is associated with
    /// </param>
mojoAjaxControlToolkit.BehaviorBase.initializeBase(this, [element]);

    this._clientStateFieldID = null;
    this._pageRequestManager = null;
    this._partialUpdateBeginRequestHandler = null;
    this._partialUpdateEndRequestHandler = null;
}
mojoAjaxControlToolkit.BehaviorBase.prototype = {
    initialize: function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>

        // TODO: Evaluate necessity
    mojoAjaxControlToolkit.BehaviorBase.callBaseMethod(this, 'initialize');
    },

    dispose: function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>
    mojoAjaxControlToolkit.BehaviorBase.callBaseMethod(this, 'dispose');

        if (this._pageRequestManager) {
            if (this._partialUpdateBeginRequestHandler) {
                this._pageRequestManager.remove_beginRequest(this._partialUpdateBeginRequestHandler);
                this._partialUpdateBeginRequestHandler = null;
            }
            if (this._partialUpdateEndRequestHandler) {
                this._pageRequestManager.remove_endRequest(this._partialUpdateEndRequestHandler);
                this._partialUpdateEndRequestHandler = null;
            }
            this._pageRequestManager = null;
        }
    },

    get_ClientStateFieldID: function() {
        /// <value type="String">
        /// ID of the hidden field used to store client state
        /// </value>
        return this._clientStateFieldID;
    },
    set_ClientStateFieldID: function(value) {
        if (this._clientStateFieldID != value) {
            this._clientStateFieldID = value;
            this.raisePropertyChanged('ClientStateFieldID');
        }
    },

    get_ClientState: function() {
        /// <value type="String">
        /// Client state
        /// </value>
        if (this._clientStateFieldID) {
            var input = document.getElementById(this._clientStateFieldID);
            if (input) {
                return input.value;
            }
        }
        return null;
    },
    set_ClientState: function(value) {
        if (this._clientStateFieldID) {
            var input = document.getElementById(this._clientStateFieldID);
            if (input) {
                input.value = value;
            }
        }
    },

    registerPartialUpdateEvents: function() {
        /// <summary>
        /// Register for beginRequest and endRequest events on the PageRequestManager,
        /// (which cause _partialUpdateBeginRequest and _partialUpdateEndRequest to be
        /// called when an UpdatePanel refreshes)
        /// </summary>

        if (Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            this._pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
            if (this._pageRequestManager) {
                this._partialUpdateBeginRequestHandler = Function.createDelegate(this, this._partialUpdateBeginRequest);
                this._pageRequestManager.add_beginRequest(this._partialUpdateBeginRequestHandler);
                this._partialUpdateEndRequestHandler = Function.createDelegate(this, this._partialUpdateEndRequest);
                this._pageRequestManager.add_endRequest(this._partialUpdateEndRequestHandler);
            }
        }
    },

    _partialUpdateBeginRequest: function(sender, beginRequestEventArgs) {
        /// <summary>
        /// Method that will be called when a partial update (via an UpdatePanel) begins,
        /// if registerPartialUpdateEvents() has been called.
        /// </summary>
        /// <param name="sender" type="Object">
        /// Sender
        /// </param>
        /// <param name="beginRequestEventArgs" type="Sys.WebForms.BeginRequestEventArgs">
        /// Event arguments
        /// </param>

        // Nothing done here; override this method in a child class
    },

    _partialUpdateEndRequest: function(sender, endRequestEventArgs) {
        /// <summary>
        /// Method that will be called when a partial update (via an UpdatePanel) finishes,
        /// if registerPartialUpdateEvents() has been called.
        /// </summary>
        /// <param name="sender" type="Object">
        /// Sender
        /// </param>
        /// <param name="endRequestEventArgs" type="Sys.WebForms.EndRequestEventArgs">
        /// Event arguments
        /// </param>

        // Nothing done here; override this method in a child class
    }
}
mojoAjaxControlToolkit.BehaviorBase.registerClass('mojoAjaxControlToolkit.BehaviorBase', Sys.UI.Behavior);


// Dynamically populates content when the populate method is called
mojoAjaxControlToolkit.DynamicPopulateBehaviorBase = function(element) {
    /// <summary>
    /// DynamicPopulateBehaviorBase is used to add DynamicPopulateBehavior funcitonality
    /// to other extenders.  It will dynamically populate the contents of the target element
    /// when its populate method is called.
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" domElement="true">
    /// DOM Element the behavior is associated with
    /// </param>
mojoAjaxControlToolkit.DynamicPopulateBehaviorBase.initializeBase(this, [element]);

    this._DynamicControlID = null;
    this._DynamicContextKey = null;
    this._DynamicServicePath = null;
    this._DynamicServiceMethod = null;
    this._cacheDynamicResults = false;
    this._dynamicPopulateBehavior = null;
    this._populatingHandler = null;
    this._populatedHandler = null;
}
mojoAjaxControlToolkit.DynamicPopulateBehaviorBase.prototype = {
    initialize: function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>

    mojoAjaxControlToolkit.DynamicPopulateBehaviorBase.callBaseMethod(this, 'initialize');

        // Create event handlers
        this._populatingHandler = Function.createDelegate(this, this._onPopulating);
        this._populatedHandler = Function.createDelegate(this, this._onPopulated);
    },

    dispose: function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>

        // Dispose of event handlers
        if (this._populatedHandler) {
            if (this._dynamicPopulateBehavior) {
                this._dynamicPopulateBehavior.remove_populated(this._populatedHandler);
            }
            this._populatedHandler = null;
        }
        if (this._populatingHandler) {
            if (this._dynamicPopulateBehavior) {
                this._dynamicPopulateBehavior.remove_populating(this._populatingHandler);
            }
            this._populatingHandler = null;
        }

        // Dispose of the placeholder control and behavior
        if (this._dynamicPopulateBehavior) {
            this._dynamicPopulateBehavior.dispose();
            this._dynamicPopulateBehavior = null;
        }
        mojoAjaxControlToolkit.DynamicPopulateBehaviorBase.callBaseMethod(this, 'dispose');
    },

    populate: function(contextKeyOverride) {
        /// <summary>
        /// Demand-create the DynamicPopulateBehavior and use it to populate the target element
        /// </summary>
        /// <param name="contextKeyOverride" type="String" mayBeNull="true" optional="true">
        /// An arbitrary string value to be passed to the web method. For example, if the element to be populated is within a data-bound repeater, this could be the ID of the current row.
        /// </param>

        // If the DynamicPopulateBehavior's element is out of date, dispose of it
        if (this._dynamicPopulateBehavior && (this._dynamicPopulateBehavior.get_element() != $get(this._DynamicControlID))) {
            this._dynamicPopulateBehavior.dispose();
            this._dynamicPopulateBehavior = null;
        }

        // If a DynamicPopulateBehavior is not available and the necessary information is, create one
        if (!this._dynamicPopulateBehavior && this._DynamicControlID && this._DynamicServiceMethod) {
            this._dynamicPopulateBehavior = $create(mojoAjaxControlToolkit.DynamicPopulateBehavior,
                {
                    "id": this.get_id() + "_DynamicPopulateBehavior",
                    "ContextKey": this._DynamicContextKey,
                    "ServicePath": this._DynamicServicePath,
                    "ServiceMethod": this._DynamicServiceMethod,
                    "cacheDynamicResults": this._cacheDynamicResults
                }, null, null, $get(this._DynamicControlID));

            // Attach event handlers
            this._dynamicPopulateBehavior.add_populating(this._populatingHandler);
            this._dynamicPopulateBehavior.add_populated(this._populatedHandler);
        }

        // If a DynamicPopulateBehavior is available, use it to populate the dynamic content
        if (this._dynamicPopulateBehavior) {
            this._dynamicPopulateBehavior.populate(contextKeyOverride ? contextKeyOverride : this._DynamicContextKey);
        }
    },

    _onPopulating: function(sender, eventArgs) {
        /// <summary>
        /// Handler for DynamicPopulate behavior's Populating event
        /// </summary>
        /// <param name="sender" type="Object">
        /// DynamicPopulate behavior
        /// </param>
        /// <param name="eventArgs" type="Sys.CancelEventArgs" mayBeNull="false">
        /// Event args
        /// </param>
        this.raisePopulating(eventArgs);
    },

    _onPopulated: function(sender, eventArgs) {
        /// <summary>
        /// Handler for DynamicPopulate behavior's Populated event
        /// </summary>
        /// <param name="sender" type="Object">
        /// DynamicPopulate behavior
        /// </param>
        /// <param name="eventArgs" type="Sys.EventArgs" mayBeNull="false">
        /// Event args
        /// </param>
        this.raisePopulated(eventArgs);
    },

    get_dynamicControlID: function() {
        /// <value type="String">
        /// ID of the element to populate with dynamic content
        /// </value>
        return this._DynamicControlID;
    },
    get_DynamicControlID: this.get_dynamicControlID,
    set_dynamicControlID: function(value) {
        if (this._DynamicControlID != value) {
            this._DynamicControlID = value;
            this.raisePropertyChanged('dynamicControlID');
            this.raisePropertyChanged('DynamicControlID');
        }
    },
    set_DynamicControlID: this.set_dynamicControlID,

    get_dynamicContextKey: function() {
        /// <value type="String">
        /// An arbitrary string value to be passed to the web method.
        /// For example, if the element to be populated is within a
        /// data-bound repeater, this could be the ID of the current row.
        /// </value>
        return this._DynamicContextKey;
    },
    get_DynamicContextKey: this.get_dynamicContextKey,
    set_dynamicContextKey: function(value) {
        if (this._DynamicContextKey != value) {
            this._DynamicContextKey = value;
            this.raisePropertyChanged('dynamicContextKey');
            this.raisePropertyChanged('DynamicContextKey');
        }
    },
    set_DynamicContextKey: this.set_dynamicContextKey,

    get_dynamicServicePath: function() {
        /// <value type="String" mayBeNull="true" optional="true">
        /// The URL of the web service to call.  If the ServicePath is not defined, then we will invoke a PageMethod instead of a web service.
        /// </value>
        return this._DynamicServicePath;
    },
    get_DynamicServicePath: this.get_dynamicServicePath,
    set_dynamicServicePath: function(value) {
        if (this._DynamicServicePath != value) {
            this._DynamicServicePath = value;
            this.raisePropertyChanged('dynamicServicePath');
            this.raisePropertyChanged('DynamicServicePath');
        }
    },
    set_DynamicServicePath: this.set_dynamicServicePath,

    get_dynamicServiceMethod: function() {
        /// <value type="String">
        /// The name of the method to call on the page or web service
        /// </value>
        /// <remarks>
        /// The signature of the method must exactly match the following:
        ///     [WebMethod]
        ///     string DynamicPopulateMethod(string contextKey)
        ///     {
        ///         ...
        ///     }
        /// </remarks>
        return this._DynamicServiceMethod;
    },
    get_DynamicServiceMethod: this.get_dynamicServiceMethod,
    set_dynamicServiceMethod: function(value) {
        if (this._DynamicServiceMethod != value) {
            this._DynamicServiceMethod = value;
            this.raisePropertyChanged('dynamicServiceMethod');
            this.raisePropertyChanged('DynamicServiceMethod');
        }
    },
    set_DynamicServiceMethod: this.set_dynamicServiceMethod,

    get_cacheDynamicResults: function() {
        /// <value type="Boolean" mayBeNull="false">
        /// Whether the results of the dynamic population should be cached and
        /// not fetched again after the first load
        /// </value>
        return this._cacheDynamicResults;
    },
    set_cacheDynamicResults: function(value) {
        if (this._cacheDynamicResults != value) {
            this._cacheDynamicResults = value;
            this.raisePropertyChanged('cacheDynamicResults');
        }
    },

    add_populated: function(handler) {
        /// <summary>
        /// Add a handler on the populated event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().addHandler("populated", handler);
    },
    remove_populated: function(handler) {
        /// <summary>
        /// Remove a handler from the populated event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().removeHandler("populated", handler);
    },
    raisePopulated: function(arg) {
        /// <summary>
        /// Raise the populated event
        /// </summary>
        /// <param name="arg" type="Sys.EventArgs">
        /// Event arguments
        /// </param>
        var handler = this.get_events().getHandler("populated");
        if (handler) handler(this, arg);
    },

    add_populating: function(handler) {
        /// <summary>
        /// Add an event handler for the populating event
        /// </summary>
        /// <param name="handler" type="Function" mayBeNull="false">
        /// Event handler
        /// </param>
        /// <returns />
        this.get_events().addHandler('populating', handler);
    },
    remove_populating: function(handler) {
        /// <summary>
        /// Remove an event handler from the populating event
        /// </summary>
        /// <param name="handler" type="Function" mayBeNull="false">
        /// Event handler
        /// </param>
        /// <returns />
        this.get_events().removeHandler('populating', handler);
    },
    raisePopulating: function(eventArgs) {
        /// <summary>
        /// Raise the populating event
        /// </summary>
        /// <param name="eventArgs" type="Sys.CancelEventArgs" mayBeNull="false">
        /// Event arguments for the populating event
        /// </param>
        /// <returns />

        var handler = this.get_events().getHandler('populating');
        if (handler) {
            handler(this, eventArgs);
        }
    }
}
mojoAjaxControlToolkit.DynamicPopulateBehaviorBase.registerClass('mojoAjaxControlToolkit.DynamicPopulateBehaviorBase', mojoAjaxControlToolkit.BehaviorBase);


mojoAjaxControlToolkit.ControlBase = function(element) {
mojoAjaxControlToolkit.ControlBase.initializeBase(this, [element]);
    this._clientStateField = null;
    this._callbackTarget = null;
    this._onsubmit$delegate = Function.createDelegate(this, this._onsubmit);
    this._oncomplete$delegate = Function.createDelegate(this, this._oncomplete);
    this._onerror$delegate = Function.createDelegate(this, this._onerror);
}

mojoAjaxControlToolkit.ControlBase.__doPostBack = function(eventTarget, eventArgument) {
    if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) {
        for (var i = 0; i < mojoAjaxControlToolkit.ControlBase.onsubmitCollection.length; i++) {
            mojoAjaxControlToolkit.ControlBase.onsubmitCollection[i]();
        }
    }
    Function.createDelegate(window, mojoAjaxControlToolkit.ControlBase.__doPostBackSaved)(eventTarget, eventArgument);
}

mojoAjaxControlToolkit.ControlBase.prototype = {
    initialize: function() {
    mojoAjaxControlToolkit.ControlBase.callBaseMethod(this, "initialize");
        // load the client state if possible
        if (this._clientStateField) {
            this.loadClientState(this._clientStateField.value);
        }
        // attach an event to save the client state before a postback or updatepanel partial postback
        if (typeof (Sys.WebForms) !== "undefined" && typeof (Sys.WebForms.PageRequestManager) !== "undefined") {
            Array.add(Sys.WebForms.PageRequestManager.getInstance()._onSubmitStatements, this._onsubmit$delegate);
            if (mojoAjaxControlToolkit.ControlBase.__doPostBackSaved == null || typeof mojoAjaxControlToolkit.ControlBase.__doPostBackSaved == "undefined") {
                mojoAjaxControlToolkit.ControlBase.__doPostBackSaved = window.__doPostBack;
                window.__doPostBack = mojoAjaxControlToolkit.ControlBase.__doPostBack;
                mojoAjaxControlToolkit.ControlBase.onsubmitCollection = new Array();
            }
            Array.add(mojoAjaxControlToolkit.ControlBase.onsubmitCollection, this._onsubmit$delegate);
        } else {
            $addHandler(document.forms[0], "submit", this._onsubmit$delegate);
        }
    },
    dispose: function() {
        if (typeof (Sys.WebForms) !== "undefined" && typeof (Sys.WebForms.PageRequestManager) !== "undefined") {
            Array.remove(mojoAjaxControlToolkit.ControlBase.onsubmitCollection, this._onsubmit$delegate);
            Array.remove(Sys.WebForms.PageRequestManager.getInstance()._onSubmitStatements, this._onsubmit$delegate);
        } else {
            $removeHandler(document.forms[0], "submit", this._onsubmit$delegate);
        }
        mojoAjaxControlToolkit.ControlBase.callBaseMethod(this, "dispose");
    },

    findElement: function(id) {
        // <summary>Finds an element within this control (ScriptControl/ScriptUserControl are NamingContainers);
        return $get(this.get_id() + '_' + id.split(':').join('_'));
    },
    get_clientStateField: function() {
        return this._clientStateField;
    },
    set_clientStateField: function(value) {
    if (this.get_isInitialized()) throw Error.invalidOperation("ExtenderBase_CannotSetClientStateField");
        if (this._clientStateField != value) {
            this._clientStateField = value;
            this.raisePropertyChanged('clientStateField');
        }
    },
    loadClientState: function(value) {
        /// <remarks>override this method to intercept client state loading after a callback</remarks>
    },
    saveClientState: function() {
        /// <remarks>override this method to intercept client state acquisition before a callback</remarks>
        return null;
    },
    _invoke: function(name, args, cb) {
        /// <summary>invokes a callback method on the server control</summary>        
        if (!this._callbackTarget) {
            throw Error.invalidOperation("ExtenderBase_ControlNotRegisteredForCallbacks");
        }
        if (typeof (WebForm_DoCallback) === "undefined") {
            throw Error.invalidOperation("ExtenderBase_PageNotRegisteredForCallbacks");
        }
        var ar = [];
        for (var i = 0; i < args.length; i++)
            ar[i] = args[i];
        var clientState = this.saveClientState();
        if (clientState != null && !String.isInstanceOfType(clientState)) {
            throw Error.invalidOperation("ExtenderBase_InvalidClientStateType");
        }
        var payload = Sys.Serialization.JavaScriptSerializer.serialize({ name: name, args: ar, state: this.saveClientState() });
        WebForm_DoCallback(this._callbackTarget, payload, this._oncomplete$delegate, cb, this._onerror$delegate, true);
    },
    _oncomplete: function(result, context) {
        result = Sys.Serialization.JavaScriptSerializer.deserialize(result);
        if (result.error) {
            throw Error.create(result.error);
        }
        this.loadClientState(result.state);
        context(result.result);
    },
    _onerror: function(message, context) {
        throw Error.create(message);
    },
    _onsubmit: function() {
        if (this._clientStateField) {
            this._clientStateField.value = this.saveClientState();
        }
        return true;
    }

}
mojoAjaxControlToolkit.ControlBase.registerClass("mojoAjaxControlToolkit.ControlBase", Sys.UI.Control);

// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.


/// <reference name="MicrosoftAjax.debug.js" />
/// <reference name="MicrosoftAjaxTimer.debug.js" />
/// <reference name="MicrosoftAjaxWebForms.debug.js" />
/// <reference path="../ExtenderBase/BaseScripts.js" />




mojoAjaxControlToolkit.AjaxRatingBehavior = function(element) {
    /// <summary>
    /// The RatingBehavior creates a sequence of stars used to rate an item
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" domElement="true">
    /// DOM element associated with the behavior
    /// </param>
mojoAjaxControlToolkit.AjaxRatingBehavior.initializeBase(this, [element]);
    this._starCssClass = null;
    this._filledStarCssClass = null;
    this._emptyStarCssClass = null;
    this._waitingStarCssClass = null;
    this._readOnly = false;
    this._ratingValue = 0;
    this._currentRating = 0;
    this._maxRatingValue = 5;
    this._tag = "";
    this._ratingDirection = 0;
    this._stars = null;
    this._callbackID = null;
    this._mouseOutHandler = Function.createDelegate(this, this._onMouseOut);
    this._starClickHandler = Function.createDelegate(this, this._onStarClick);
    this._starMouseOverHandler = Function.createDelegate(this, this._onStarMouseOver);
    this._keyDownHandler = Function.createDelegate(this, this._onKeyDownBack);
    this._autoPostBack = false;
    this._jsonUrl = "";
    this._contentId = "";
    this._totalVotesElementId = "";
    this._commentsEnabled = false;
}
mojoAjaxControlToolkit.AjaxRatingBehavior.prototype = {
    initialize: function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>
        mojoAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'initialize');
        var elt = this.get_element();
        this._stars = [];
        for (var i = 1; i <= this._maxRatingValue; i++) {
            starElement = $get(elt.id + '_Star_' + i);
            starElement.value = i;
            Array.add(this._stars, starElement);
            $addHandler(starElement, 'click', this._starClickHandler);
            $addHandler(starElement, 'mouseover', this._starMouseOverHandler);
        }
        $addHandler(elt, 'mouseout', this._mouseOutHandler);
        $addHandler(elt, "keydown", this._keyDownHandler);
        this._update();
        //alert(this._contentId);
    },

    dispose: function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>

        var elt = this.get_element();
        if (this._stars) {
            for (var i = 0; i < this._stars.length; i++) {
                var starElement = this._stars[i];
                $removeHandler(starElement, 'click', this._starClickHandler);
                $removeHandler(starElement, 'mouseover', this._starMouseOverHandler);
            }
            this._stars = null;
        }
        $removeHandler(elt, 'mouseout', this._mouseOutHandler);
        $removeHandler(elt, "keydown", this._keyDownHandler);
        mojoAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'dispose');
    },

    _onError: function(message, context) {
        /// <summary>
        /// Error handler for the callback
        /// </summary>
        /// <param name="message" type="String">
        /// Error message
        /// </param>
        /// <param name="context" type="Object">
        /// Context
        /// </param>
        //alert(String.format(AjaxControlToolkit.Resources.Rating_CallbackError, message));
        alert(String.format("callback error", message));
    },

    _receiveServerData: function(arg, context) {
        /// <summary>
        /// Handler for successful return from callback
        /// </summary>
        /// <param name="arg" type="Object">
        /// Argument
        /// </param>
        /// <param name="context" type="Object">
        /// Context
        /// </param>
        context._waitingMode(false);
        context.raiseEndClientCallback(arg);
    },

    _onMouseOut: function(e) {
        /// <summary>
        /// Handler for a star's mouseout event
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">
        /// Event info
        /// </param>

        if (this._readOnly) {
            return;
        }
        this._currentRating = this._ratingValue;
        this._update();
        this.raiseMouseOut(this._currentRating);
    },

    _onStarClick: function(e) {
        /// <summary>
        /// Handler for a star's click event
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">
        /// Event info
        /// </param>
        if (this._readOnly) {
            return;
        }
        this.set_Rating(this._currentRating);
//        if (this._ratingValue != this._currentRating) {
//            this.set_Rating(this._currentRating);
//        }
    },

    _onStarMouseOver: function(e) {
        /// <summary>
        /// Handler for a star's mouseover event
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">
        /// Event info
        /// </param>
        if (this._readOnly) {
            return;
        }
        if (this._ratingDirection == 0) {
            this._currentRating = e.target.value;
        } else {
            this._currentRating = this._maxRatingValue + 1 - e.target.value;
        }
        this._update();
        this.raiseMouseOver(this._currentRating);
    },


    _onKeyDownBack: function(ev) {
        /// <summary>
        /// Handler for a star's keyDown event
        /// </summary>
        /// <param name="ev" type="Sys.UI.DomEvent">
        /// Event info
        /// </param>
        if (this._readOnly) {
            return;
        }
        var k = ev.keyCode ? ev.keyCode : ev.rawEvent.keyCode;
        if ((k == Sys.UI.Key.right) || (k == Sys.UI.Key.up)) {
            this._currentRating = Math.min(this._currentRating + 1, this._maxRatingValue);
            this.set_Rating(this._currentRating);
            ev.preventDefault();
            ev.stopPropagation();
        } else if ((k == Sys.UI.Key.left) || (k == Sys.UI.Key.down)) {
            this._currentRating = Math.max(this._currentRating - 1, 1);
            this.set_Rating(this._currentRating);
            ev.preventDefault();
            ev.stopPropagation();
        }
    },

    _waitingMode: function(activated) {
        /// <summary>
        /// Update the display to indicate whether or not we are waiting
        /// </summary>
        /// <param name="activated" type="Boolean">
        /// Whether or not we are waiting
        /// </param>

        for (var i = 0; i < this._maxRatingValue; i++) {
            var starElement;
            if (this._ratingDirection == 0) {
                starElement = this._stars[i];
            } else {
                starElement = this._stars[this._maxRatingValue - i - 1];
            }
            if (this._currentRating > i) {
                if (activated) {
                    Sys.UI.DomElement.removeCssClass(starElement, this._filledStarCssClass);
                    Sys.UI.DomElement.addCssClass(starElement, this._waitingStarCssClass);
                } else {
                    Sys.UI.DomElement.removeCssClass(starElement, this._waitingStarCssClass);
                    Sys.UI.DomElement.addCssClass(starElement, this._filledStarCssClass);
                }
            } else {
                Sys.UI.DomElement.removeCssClass(starElement, this._waitingStarCssClass);
                Sys.UI.DomElement.removeCssClass(starElement, this._filledStarCssClass);
                Sys.UI.DomElement.addCssClass(starElement, this._emptyStarCssClass);
            }
        }
    },

    _update: function() {
        /// <summary>
        /// Update the display
        /// </summary>
        // Update title attribute element
        var elt = this.get_element();
        $get(elt.id + "_A").title = this._currentRating;
        

        for (var i = 0; i < this._maxRatingValue; i++) {
            var starElement;
            if (this._ratingDirection == 0) {
                starElement = this._stars[i];
            } else {
                starElement = this._stars[this._maxRatingValue - i - 1];
            }

            
            if (this._currentRating > i) {
                Sys.UI.DomElement.removeCssClass(starElement, this._emptyStarCssClass);
                Sys.UI.DomElement.addCssClass(starElement, this._filledStarCssClass);
            }
            else {
                Sys.UI.DomElement.removeCssClass(starElement, this._filledStarCssClass);
                Sys.UI.DomElement.addCssClass(starElement, this._emptyStarCssClass);
            }
        }
    },

    add_Rated: function(handler) {
        /// <summary>
        /// Add a handler to the rated event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().addHandler("Rated", handler);
    },
    remove_Rated: function(handler) {
        /// <summary>
        /// Remove a handler from the rated event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().removeHandler("Rated", handler);
    },
    raiseRated: function(rating) {
        /// <summary>
        /// Raise the rated event
        /// </summary>
        /// <param name="rating" type="Number" integer="true">
        /// Rating
        /// </param>
        var handler = this.get_events().getHandler("Rated");
        if (handler) {
            handler(this, new mojoAjaxControlToolkit.AjaxRatingEventArgs(rating));
        }
    },

    add_MouseOver: function(handler) {
        /// <summary>
        /// Add a handler to the MouseMove event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().addHandler("MouseOver", handler);
    },
    remove_MouseOver: function(handler) {
        /// <summary>
        /// Remove a handler from the MouseOver event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().removeHandler("MouseOver", handler);
    },
    raiseMouseOver: function(rating_tmp) {
        /// <summary>
        /// Raise the MouseOver event
        /// </summary>
        /// <param name="eventArgs" type="">
        /// eventArgs
        /// </param>
        var handler = this.get_events().getHandler("MouseOver");
        if (handler) {
            handler(this, new mojoAjaxControlToolkit.AjaxRatingEventArgs(rating_tmp));
        }
    },

    add_MouseOut: function(handler) {
        /// <summary>
        /// Add a handler to the MouseOut event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().addHandler("MouseOut", handler);
    },
    remove_MouseOut: function(handler) {
        /// <summary>
        /// Remove a handler from the MouseOut event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().removeHandler("MouseOut", handler);
    },
    raiseMouseOut: function(rating_old) {
        /// <summary>
        /// Raise the MouseOut event
        /// </summary>
        /// <param name="eventArgs" type="">
        /// eventArgs
        /// </param>
        var handler = this.get_events().getHandler("MouseOut");
        if (handler) {
            handler(this, new mojoAjaxControlToolkit.AjaxRatingEventArgs(rating_old));
        }
    },

    add_EndClientCallback: function(handler) {
        /// <summary>
        /// Add a handler to the EndClientCallback event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().addHandler("EndClientCallback", handler);
    },
    remove_EndClientCallback: function(handler) {
        /// <summary>
        /// Remove a handler from the EndClientCallback event
        /// </summary>
        /// <param name="handler" type="Function">
        /// Handler
        /// </param>
        this.get_events().removeHandler("EndClientCallback", handler);
    },
    raiseEndClientCallback: function(result) {
        /// <summary>
        /// Raise the EndClientCallback event
        /// </summary>
        /// <param name="result" type="String">
        /// Callback result
        /// </param>
        var handler = this.get_events().getHandler("EndClientCallback");
        if (handler) {
            handler(this, new mojoAjaxControlToolkit.AjaxRatingCallbackResultEventArgs(result));
        }
    },

    get_AutoPostBack: function() {
        return this._autoPostBack;
    },

    set_AutoPostBack: function(value) {
        this._autoPostBack = value;
    },

    get_Stars: function() {
        /// <value type="Array" elementType="Sys.UI.DomElement" elementDomElement="true">
        /// Elements for the displayed stars
        /// </value>
        return this._stars;
    },

    get_Tag: function() {
        /// <value type="String">
        /// A custom parameter to pass to the ClientCallBack
        /// </value>
        return this._tag;
    },
    set_Tag: function(value) {
        if (this._tag != value) {
            this._tag = value;
            this.raisePropertyChanged('Tag');
        }
    },

    // property added by 
    get_JsonUrl: function() {
        /// <value type="String">
        ///json service url
        /// </value>
        return this._jsonUrl;
    },
    set_JsonUrl: function(value) {
        if (this._jsonUrl != value) {
            this._jsonUrl = value;
            this.raisePropertyChanged('JsonUrl');
        }
    },

    // property added by 
    get_ContentId: function() {
        /// <value type="String">
        ///content id
        /// </value>
        return this._contentId;
    },
    set_ContentId: function(value) {
        if (this._contentId != value) {
            this._contentId = value;
            this.raisePropertyChanged('ContentId');
        }
    },

    //_totalVotesElementId
    // property added by 
    get_TotalVotesElementId: function() {
        /// <value type="String">
        ///content id
        /// </value>
        return this._totalVotesElementId;
    },
    set_TotalVotesElementId: function(value) {
        if (this._totalVotesElementId != value) {
            this._totalVotesElementId = value;
            this.raisePropertyChanged('TotalVotesElementId');
        }
    },

    get_CallbackID: function() {
        /// <value type="String">
        /// ID of the ClientCallBack
        /// </value>
        return this._callbackID;
    },
    set_CallbackID: function(value) {
        this._callbackID = value;
    },

    get_RatingDirection: function() {
        /// <value type="Number" integer="true">
        /// RatingDirection - Orientation of the stars (LeftToRightTopToBottom or RightToLeftBottomToTop)
        /// </value>
        /// TODO: We should create an enum for this
        return this._ratingDirection;
    },
    set_RatingDirection: function(value) {
        if (this._ratingDirection != value) {
            this._ratingDirection = value;
            if (this.get_isInitialized()) {
                this._update();
            }
            this.raisePropertyChanged('RatingDirection');
        }
    },

    get_EmptyStarCssClass: function() {
        /// <value type="String">
        /// CSS class for a star in empty mode
        /// </value>
        return this._emptyStarCssClass;
    },
    set_EmptyStarCssClass: function(value) {
        if (this._emptyStarCssClass != value) {
            this._emptyStarCssClass = value;
            this.raisePropertyChanged('EmptyStarCssClass');
        }
    },

    get_FilledStarCssClass: function() {
        /// <value type="String">
        /// CSS class for star in filled mode
        /// </value>
        return this._filledStarCssClass;
    },
    set_FilledStarCssClass: function(value) {
        if (this._filledStarCssClass != value) {
            this._filledStarCssClass = value;
            this.raisePropertyChanged('FilledStarCssClass');
        }
    },

    get_WaitingStarCssClass: function() {
        /// <value type="String">
        /// CSS class for a star in waiting mode
        /// </value>
        return this._waitingStarCssClass;
    },
    set_WaitingStarCssClass: function(value) {
        if (this._waitingStarCssClass != value) {
            this._waitingStarCssClass = value;
            this.raisePropertyChanged('WaitingStarCssClass');
        }
    },



    get_Rating: function() {
        /// <value type="Number" integer="true">
        /// Current rating value
        /// </value>
        this._ratingValue = mojoAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'get_ClientState');
        if (this._ratingValue == '')
            this._ratingValue = null;
        return this._ratingValue;
    },
    set_Rating: function(value) {
        this._ratingValue = value;
        this._currentRating = value;
        if (this.get_isInitialized()) {
            if ((value < 0) || (value > this._maxRatingValue)) {
                return;
            }

            this._update();

            mojoAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'set_ClientState', [this._ratingValue]);
            this.raisePropertyChanged('Rating');
            this.raiseRated(this._currentRating);
            // postback will be made by the comments button
            if (this._commentsEnabled) { return; }

            this._waitingMode(true);

            var args = this._currentRating + ";" + this._tag;
            var id = this._callbackID;
            var result = null;
            var votes = null;

            //  : here is where I added a 3rd option to post to a json service url
            if ((this._jsonUrl.length > 0) && (this._contentId.length == 36)) {
                var ratingData = "cid=" + this._contentId + "&r=" + this._ratingValue;

                $.ajax({
                    type: "POST",
                    async: false,
                    processData: false,
                    url: this._jsonUrl,
                    data: ratingData,
                    contentType: "text/javascript",
                    dataType: "json",
                    success: function(data) {
                        result = data.avg;
                        votes = data.votes;
                        //alert(data.avg);

                    }
                });

                if ((this._totalVotesElementId.length > 0) && (votes != null)) {
                    $('#' + this._totalVotesElementId).html(votes);
                    //alert('updated votes');
                }
                this._waitingMode(false);
                this._currentRating = result;
                this._ratingValue = result;
                this._update();

            }
            else if (this._autoPostBack) {
                __doPostBack(id, args);
            }
            else {
                WebForm_DoCallback(id, args, this._receiveServerData, this, this._onError, true)
            }

        }
        
    },

    get_MaxRating: function() {
        /// <value type="Number" integer="true">
        /// Maximum rating value
        /// </value>
        return this._maxRatingValue;
    },
    set_MaxRating: function(value) {
        if (this._maxRatingValue != value) {
            this._maxRatingValue = value;
            this.raisePropertyChanged('MaxRating');
        }
    },


    get_ReadOnly: function() {
        /// <value type="Boolean">
        /// Whether or not the rating can be changed
        /// </value>
        return this._readOnly;
    },
    set_ReadOnly: function(value) {
        if (this._readOnly != value) {
            this._readOnly = value;
            this.raisePropertyChanged('ReadOnly');
        }
    },

    // property added by 
    get_CommentsEnabled: function() {
        /// <value type="Boolean">
        /// Whether or not the rating can be changed
        /// </value>
        return this._commentsEnabled;
    },
    set_CommentsEnabled: function(value) {
        if (this._commentsEnabled != value) {
            this._commentsEnabled = value;
            this.raisePropertyChanged('CommentsEnabled');
        }
    },



    get_StarCssClass: function() {
        /// <value type="String">
        /// CSS class for a visible star
        /// </value>
        return this._starCssClass;
    },
    set_StarCssClass: function(value) {
        if (this._starCssClass != value) {
            this._starCssClass = value;
            this.raisePropertyChanged('StarCssClass');
        }
    }
}
mojoAjaxControlToolkit.AjaxRatingBehavior.registerClass('mojoAjaxControlToolkit.AjaxRatingBehavior', mojoAjaxControlToolkit.BehaviorBase);


mojoAjaxControlToolkit.AjaxRatingEventArgs = function(rating) {
    /// <summary>
    /// Event arguments for the RatingBehavior's rated event
    /// </summary>
    /// <param name="rating" type="Number" integer="true">
    /// Rating
    /// </param>
mojoAjaxControlToolkit.AjaxRatingEventArgs.initializeBase(this);

    this._rating = rating;
}
mojoAjaxControlToolkit.AjaxRatingEventArgs.prototype = {
    get_Rating: function() {
        /// <value type="Number" integer="true">
        /// Rating
        /// </value>
        return this._rating;
    }
}
mojoAjaxControlToolkit.AjaxRatingEventArgs.registerClass('mojoAjaxControlToolkit.AjaxRatingEventArgs', Sys.EventArgs);


mojoAjaxControlToolkit.AjaxRatingCallbackResultEventArgs = function(result) {
    /// <summary>
    /// Event arguments for the RatingBehavior's EndClientCallback event
    /// </summary>
    /// <param name="result" type="Object">
    /// Callback result
    /// </param>
mojoAjaxControlToolkit.AjaxRatingCallbackResultEventArgs.initializeBase(this);

    this._result = result;
}
mojoAjaxControlToolkit.AjaxRatingCallbackResultEventArgs.prototype = {
    get_CallbackResult: function() {
        /// <value type="Object">
        /// Callback result
        /// </value>
        return this._result;
    }
}
mojoAjaxControlToolkit.AjaxRatingCallbackResultEventArgs.registerClass('mojoAjaxControlToolkit.AjaxRatingCallbackResultEventArgs', Sys.EventArgs);
