function StartTable() {
    return '<table class="table table-striped table-advance table-hover result-table"><tbody>';
}

function EndTable() {
    return '</tbody></table>';
}


function StartTableRow() {
    return '<tr>';
}

function EndTableRow() {
    return '</tr>';
}



function TableHeader(Name, Width) {
    return '<th class="table-header table-th table-row-width-' + Width + '" >' + Name + '</th>';
}

function TableData(Data)
{
    return '<td class="modalSearchRow"><label class="table-label customCursor">' + Data + '</label></td>';
}

function StartTableRowSelectionModalPopup(Parameter)
{
    return '<tr class="customCursor" onclick="SelectionModalPopup(' + Parameter + ');">';
}