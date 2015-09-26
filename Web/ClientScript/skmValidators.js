function CheckBoxValidatorEvaluateIsValid(val)
{
    var control = document.getElementById(val.controltovalidate);
    var mustBeChecked = Boolean(val.mustBeChecked);

    return control.checked == mustBeChecked;
}

function CheckBoxListValidatorEvaluateIsValid(val)
{
    var control = document.getElementById(val.controltovalidate);
    var minimumNumberOfSelectedCheckBoxes = parseInt(val.minimumNumberOfSelectedCheckBoxes);

    var selectedItemCount = 0;
    var liIndex = 0;
    var currentListItem = document.getElementById(control.id + '_' + liIndex.toString());
    while (currentListItem != null)
    {
        if (currentListItem.checked) selectedItemCount++;
        liIndex++;
        currentListItem = document.getElementById(control.id + '_' + liIndex.toString());
    }
    
    return selectedItemCount >= minimumNumberOfSelectedCheckBoxes;
}