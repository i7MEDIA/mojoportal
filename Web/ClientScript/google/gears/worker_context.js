// This sets up the default functionality in workers. Work done in an anonymous
// function to avoid poluting the global scope.
(function() {
  var parentId;
  var messageHandlers = {};

  // workerParent is the object workers should use to talk to the scope that
  // created them.
  google.gears.workerParent = {
    // Set the handler for different message types
    setMessageHandler: function(type, handler) {
      messageHandlers[type] = handler;
    },

    // Send a message to the worker's parent
    sendMessage: function(message, args) {
      var message = "message," + message;
      if (typeof args != "undefined" && args !== null) {
        message += "," + args.toJSONString();
      }

      google.gears.workerPool.sendMessage(message, parentId);
    },

    // Report an error to the worker's parent
    sendError: function(e) {
      google.gears.workerPool.sendMessage("error," + e, parentId);
    }
  };

  google.gears.workerPool.onmessage = function(msg, senderId) {
    var msgParts = msg.match(/([^,]+)(,([^,]+))?(,(.+))?/);
    if (msgParts) {
      if (msgParts[1] == "init") {
        parentId = senderId;
        return;
      }
      
      if (msgParts[1] == "message") {
        if (messageHandlers[msgParts[3]]) {
          try {
            messageHandlers[msgParts[3]](msgParts[5].parseJSON());
            return;
          } catch (e) {
            google.gears.workerParent.sendError(e.message);
            return;
          }
        }

        google.gears.workerParent.sendError(
          "Unexpected message: " + msgParts[3]);
        return;
      }
    }

    google.gears.workerParent.sendError(
      "Internal worker error - Unexpected message: " + msg);
  };
})();
