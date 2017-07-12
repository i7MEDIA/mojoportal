// based on Sys.UI._UpdateProgress
// modified by  to work with soan instead of div
// used in /Controls/UpdateProgressSpan.cs which is a modified version of UpdateProgress from the Mono project
// last modified 2009-10-30

Type.registerNamespace("mojo");

mojo._UpdateProgress = function mojo$_UpdateProgress(element) {
    mojo._UpdateProgress.initializeBase(this,[element]);
    this._displayAfter = 500;
    this._dynamicLayout = true;
    this._associatedUpdatePanelId = null;
    this._beginRequestHandlerDelegate = null;
    this._startDelegate = null;
    this._endRequestHandlerDelegate = null;
    this._pageRequestManager = null;
    this._timerCookie = null;
}
    function mojo$_UpdateProgress$get_displayAfter() {
        /// <value type="Number" locid="P:J#mojo._UpdateProgress.displayAfter"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._displayAfter;
    }
    function mojo$_UpdateProgress$set_displayAfter(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Number}]);
        if (e) throw e;
        this._displayAfter = value;
    }
    function mojo$_UpdateProgress$get_dynamicLayout() {
        /// <value type="Boolean" locid="P:J#mojo._UpdateProgress.dynamicLayout"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._dynamicLayout;
    }
    function mojo$_UpdateProgress$set_dynamicLayout(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: Boolean}]);
        if (e) throw e;
        this._dynamicLayout = value;
    }
    function mojo$_UpdateProgress$get_associatedUpdatePanelId() {
        /// <value type="String" mayBeNull="true" locid="P:J#mojo._UpdateProgress.associatedUpdatePanelId"></value>
        if (arguments.length !== 0) throw Error.parameterCount();
        return this._associatedUpdatePanelId;
    }
    function mojo$_UpdateProgress$set_associatedUpdatePanelId(value) {
        var e = Function._validateParams(arguments, [{name: "value", type: String, mayBeNull: true}]);
        if (e) throw e;
        this._associatedUpdatePanelId = value;
    }
    function mojo$_UpdateProgress$_clearTimeout() {
        if (this._timerCookie) {
            window.clearTimeout(this._timerCookie);
            this._timerCookie = null;
        }
    }
    function mojo$_UpdateProgress$_handleBeginRequest(sender, arg) {
        var curElem = arg.get_postBackElement();
        var showProgress = !this._associatedUpdatePanelId; 
        while (!showProgress && curElem) {
            if (curElem.id && this._associatedUpdatePanelId === curElem.id) {
                showProgress = true; 
            }
            curElem = curElem.parentNode; 
        } 
        if (showProgress) {
            this._timerCookie = window.setTimeout(this._startDelegate, this._displayAfter);
        }
    }
    function mojo$_UpdateProgress$_startRequest() {
        if (this._pageRequestManager.get_isInAsyncPostBack()) {
            if (this._dynamicLayout) this.get_element().style.display = 'inline';
            else this.get_element().style.visibility = 'visible';
        }
        this._timerCookie = null;
    }
    function mojo$_UpdateProgress$_handleEndRequest(sender, arg) {
        if (this._dynamicLayout) this.get_element().style.display = 'none';
        else this.get_element().style.visibility = 'hidden';
        this._clearTimeout();
    }
    function mojo$_UpdateProgress$dispose() {
        if (this._beginRequestHandlerDelegate !== null) {
            this._pageRequestManager.remove_beginRequest(this._beginRequestHandlerDelegate);
            this._pageRequestManager.remove_endRequest(this._endRequestHandlerDelegate);
            this._beginRequestHandlerDelegate = null;
            this._endRequestHandlerDelegate = null;
        }
        this._clearTimeout();
        mojo._UpdateProgress.callBaseMethod(this,"dispose");
    }
    function mojo$_UpdateProgress$initialize() {
        mojo._UpdateProgress.callBaseMethod(this, 'initialize');
    	this._beginRequestHandlerDelegate = Function.createDelegate(this, this._handleBeginRequest);
    	this._endRequestHandlerDelegate = Function.createDelegate(this, this._handleEndRequest);
    	this._startDelegate = Function.createDelegate(this, this._startRequest);
    	if (Sys.WebForms && Sys.WebForms.PageRequestManager) {
           this._pageRequestManager = Sys.WebForms.PageRequestManager.getInstance();
    	}
    	if (this._pageRequestManager !== null ) {
    	    this._pageRequestManager.add_beginRequest(this._beginRequestHandlerDelegate);
    	    this._pageRequestManager.add_endRequest(this._endRequestHandlerDelegate);
    	}
    }
mojo._UpdateProgress.prototype = {
    get_displayAfter: mojo$_UpdateProgress$get_displayAfter,
    set_displayAfter: mojo$_UpdateProgress$set_displayAfter,
    get_dynamicLayout: mojo$_UpdateProgress$get_dynamicLayout,
    set_dynamicLayout: mojo$_UpdateProgress$set_dynamicLayout,
    get_associatedUpdatePanelId: mojo$_UpdateProgress$get_associatedUpdatePanelId,
    set_associatedUpdatePanelId: mojo$_UpdateProgress$set_associatedUpdatePanelId,
    _clearTimeout: mojo$_UpdateProgress$_clearTimeout,
    _handleBeginRequest: mojo$_UpdateProgress$_handleBeginRequest,
    _startRequest: mojo$_UpdateProgress$_startRequest,
    _handleEndRequest: mojo$_UpdateProgress$_handleEndRequest,
    dispose: mojo$_UpdateProgress$dispose,
    initialize: mojo$_UpdateProgress$initialize
}
mojo._UpdateProgress.registerClass('mojo._UpdateProgress', Sys.UI.Control);
