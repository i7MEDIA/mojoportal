/**
 * Convenience wrapper for Gears WorkerPool with the following features:
 * - Sets up default error handling for workers
 * - Allows workers to depend on external libraries
 * - Support for named messages
 * - Implement transparent json serialization/deserialization for messages
 *
 * See test.html and mylib.js for example usage.
 */
function Worker2() {
  this.code_ = [];
  this.libs_ = ["json.js", "worker_context.js"];
  this.gotLibs_ = false;
  this.pendingMessages_ = [];
  this.messageHandlers_ = {};
}

/**
 * Create a worker and populate with the contents of one or more js files.
 */
Worker2.prototype.load = function(libs) {
  for (var i = 0; i < libs.length; i++) {
    this.libs_.push(libs[i]);
  }

  this.getCode_();
};

/**
 * Set a function to handle a named message.
 */
Worker2.prototype.setMessageHandler = function(type, fnHandler) {
  this.messageHandlers_[type] = fnHandler;
};

/**
 * Helper to get the code for all the libraries.
 */
Worker2.prototype.getCode_ = function() {
  var self = this;
  var aborters = [];
  var numSuccess = 0;

  // Cancel all remaining requests to get code. Used when there is an error in
  // a request.
  var cancel = function() {
    for (var i = 0; i < self.libs_.length; i++) {
      // If there is an aborter function, call it. We remove abort functions
      // when a call completes successfully.
      if (aborters[i]) {
        aborters[i]();
      }
    }
  };

  var getLib = function(i) {
    aborters[i] = doRequest("GET", self.libs_[i], null, 
      function(status, statusText,  body) {
        if (200 != status) {
          cancel();
          self.onerror("Could not get lib '" + self.libs_[i] + "': " + 
                       status + " " + statusText);
          return;
        }

        // save the code we got.
        self.code_[i] = body;

        // clear the aborter because we were successful.
        aborters[i] = null;

        if (++numSuccess == self.libs_.length) {
          self.initWorker_();
        }
      }
    );
  };

  for (var i = 0; i < this.libs_.length; i++) {
    getLib(i);
  }
};

/**
 * Helper to initialize the internal workerpool object.
 */
Worker2.prototype.initWorker_ = function() {
  try {
    var self = this;
    this.wp_ = google.gears.factory.create('beta.workerpool', '1.0');
    this.wp_.onmessage = function(message, senderId) {
      // Need to handle all errors to workaround issue:
      // http://code.google.com/p/google-gears/issues/detail?id=33
      try {
        self.handleWorkerMessage_(message);
      } catch (e) {
        alert("Error in workerpool onmessage: " + e.message);
      }
    };
    this.workerId_ = this.wp_.createWorker(this.code_.join("\n"));

    // Send the init message. All this does is set up the parent id.
    // We need to do this in case the worker wants to start sending us messages
    // without us first sending one.
    this.wp_.sendMessage("init", this.workerId_);

    // Once this occurs we are ready to start processing messages for real.
    this.gotLibs_ = true;

    // Send any queued messages
    for (var i = 0, msg; msg = this.pendingMessages_[i]; i++) {
      this.sendMessage(msg.message, msg.args);
    }
  } catch (e) {
    if (e.constructor != Error) {
      throw new Error(e.message);
    } else {
      throw e;
    }
  }
};

/**
 * Send a message to the worker.
 * @param {String} The name of message to send.
 * @param {Object} An object to send as an argument to the message. Can be any
 * JSON-compatible object.
 */
Worker2.prototype.sendMessage = function(message, args) {
  if (!this.gotLibs_) {
    this.pendingMessages_.push({message: message, args: args});
  } else {
    if (typeof args != "undefined") {
      message += "," + args.toJSONString();
    }

    this.wp_.sendMessage("message," + message, this.workerId_);
  }
};

/**
 * Helper that handles raw incoming messages from the worker.
 */
Worker2.prototype.handleWorkerMessage_ = function(msg) {
  var msgParts = msg.match(/([^,]+)(,([^,]+))?(,(.+))?/);

  // We are especially delicate here because of:
  // http://code.google.com/p/google-gears/issues/detail?id=32&can=2&q=crash#c1
  if (msgParts) {
    if (msgParts[1] == "message") {
      if (this.messageHandlers_[msgParts[3]]) {
        this.messageHandlers_[msgParts[3]](msgParts[5].parseJSON());
        return;
      }

      throw new Error("Unexpected message: " + msgParts[3]);
    }

    if (msgParts[1] == "error") {
      this.onerror(msgParts[3]);
      return;
    }
  }

  throw new Error("Worker internal error - unexpected message: " + msg);
};

/**
 * Override to handle messages from workers.
 * @param {String} The name of the message that was sent.
 * @param {Object} The message argument that was sent, if any.
 */
Worker2.prototype.onmessage = function(message, arg) {
  // User implements this. Default implementation just logs if Firebug is
  // installed.
  alert("Received message from worker: " +arg);
};

/**
 * Override to handle worker errors in a special way. Default implementation
 * just throws them.
 * @param {String} error message from worker.
 */
Worker2.prototype.onerror = function(msg) {
  // Use setTimeout to escape stack and report to error ui.
  window.setTimeout(function() {
    throw new Error("Worker error: " + msg);
  }, 0);
};
