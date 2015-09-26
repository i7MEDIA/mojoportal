
  var prmManager = Sys.WebForms.PageRequestManager.getInstance();
 
  prmManager.add_initializeRequest(InitializeRequest);
  prmManager.add_endRequest(EndRequest);
 
  // Executed anytime an async postback occurs.
  function InitializeRequest(sender, args) 
  {
    
    // Get a reference to the element that raised the postback,
    //   and disables it.
    $get(args._postBackElement.id).disabled = true;
  }
 
  // Executed when the async postback completes.
  function EndRequest(sender, args) 
  {
    // Change the Container div's class back to .Normal.
    $get('Container').className = 'Normal';
 
    // Get a reference to the element that raised the postback
    //   which is completing, and enable it.
    $get(sender._postBackSettings.sourceElement.id).disabled = false;
  }
