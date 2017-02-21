<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSmartGrid.ascx.cs" Inherits="MyTherapy.Web.ucSmartGrid" %>
<style type="text/css">
    #divSmartGrid {
        text-align: center;
        font-family: Verdana,Arial,Helvetica,sans-serif;
        font-size: 11px;
        width: 100%;
    }

    .dataTables_selectedRows, .dataTables_toggleCheckboxes {
        text-align: center;
    }

    .dataTables_selectedRows span {
        font-size: 14px !important;
    }

    #divSmartGrid input[type="radio"], #divSmartGrid input[type="checkbox"] {
        display: none;
    }

    #divSmartGrid input[type="checkbox"] + span:before {
        font-family: 'FontAwesome';
        padding-right: 3px;
        font-size: 14px;
    }

    #divSmartGrid input[type="checkbox"] + span:before {
        content: "\f096"; /* check-empty */
    }

    #divSmartGrid input[type="checkbox"]:checked + span:before {
        content: "\f046"; /* check */
        color: Green;
    }
</style>
<script type="text/javascript">
    /* Custom Render */
    var mRenderFaDetail = function (oObj) {
        return '<div title=\"Dettaglio\" style=\"text-align: center; font-size: 18px;\"><i style=\"text-align:center;\" class=\"fa fa-search\"></i></div>';
    };
    var mRenderFaEdit = function (oObj) {
        return '<div title=\"Modifica\" style=\"text-align: center; font-size: 18px;\"><i style=\"text-align:center;\" class=\"fa fa-pencil\"></i></div>';
    };
    var mRenderFaDelete = function (oObj) {
        return '<div title=\"Elimina\" style=\"text-align: center; font-size: 18px;\"><i style=\"text-align:center;\" class=\"fa fa-trash\"></i></div>';
    };
    var renderCheckBox = function (data, type, full, meta) {
        var mADataField = full['<%= this.MultipleActionDataField %>'];
        var mADataValue = '<%= this.MultipleActionDataValue %>'.toLowerCase();
        var checked = (mADataField && mADataValue == "true") || (!mADataField && mADataValue == "false");
        return '<input type="checkbox" class="chkSmartGrid" onclick="setRowState(this)" ' + (checked ? 'checked' : '') + ' value="' + data.toString() + '"><span class="fakeCheck" onclick="fakeCheckClick(this)"></span>';
    };
    var renderDateTime = function (data, type, full, meta) {
        var datetime = new Date(parseInt(data.toString().substr(6)));
        return (pad(datetime.getDate(), 2) + '-' + pad(datetime.getMonth() + 1, 2) + '-' + datetime.getFullYear());
    };
    function pad(str, max) {
        str = str.toString();
        return str.length < max ? pad("0" + str, max) : str;
    }
    var renderCurrency = function (data, type, full, meta) {
        return '€ ' + parseFloat(data).toFixed(2);
    };
    var renderDecimal2Places = function (data, type, full, meta) {
        return parseFloat(data).toFixed(2);
    };
    var renderDecimal4Places = function (data, type, full, meta) {
        return parseFloat(data).toFixed(4);
    };

    var fnCreatedCustom = function (nTd, sData, oData, iRow, iCol) {
        var dataRowKey = '<%= this.DataRowKey %>';
        $(nTd).attr('data-' + dataRowKey, oData[dataRowKey]);
        if (fnCreatedTooltip)
            fnCreatedTooltip(nTd, oData);
    };

    var fnCreatedTooltip;

    var detailClass = '<%= this.DetailColumnClass %>';
    var editClass = '<%= this.EditColumnClass %>';
    var deleteClass = '<%= this.DeleteColumnClass %>';
    var multipleActionClass = '<%= this.MultipleActionColumnClass %>';

    function fakeCheckClick(el) {
        $(el).prev("input[type='checkbox']").click();
    }

    function RenderColumns(value, index, ar) {
        var currToolTipField;
        fnCreatedTooltip = undefined;
        if (value.sClass) {
            currToolTipField = value.sClass;
        }

        switch (value.className) {
            case detailClass:
                value.mRender = mRenderFaDetail;
                value.fnCreatedCell = fnCreatedCustom;
                value.className += " noPrint";
                break;
            case editClass:
                value.mRender = mRenderFaEdit;
                value.fnCreatedCell = fnCreatedCustom;
                value.className += " noPrint";
                break;
            case deleteClass:
                value.mRender = mRenderFaDelete;
                value.fnCreatedCell = fnCreatedCustom;
                value.className += " noPrint";
                break;
            case multipleActionClass:
                value.fnCreatedCell = fnCreatedCustom;
                value.className += " noPrint";
                break;
        }
        if (currToolTipField) {
            if (value.fnCreatedCell && typeof value.fnCreatedCell == 'function') {
                fnCreatedTooltip = function (nTd, oData) {
                    if (oData[currToolTipField])
                        $(nTd).attr('title', oData[currToolTipField]);
                };
            }
            else {
                value.fnCreatedCell = function (nTd, sData, oData, iRow, iCol) {
                    if (oData[currToolTipField])
                        $(nTd).attr('title', oData[currToolTipField]);
                };
            }
        }
    }

    function RenderColumnsDefs(value, index, ar) {
        switch (value.render) {
            case "CheckBox":
                value.render = renderCheckBox;
                break;
            case "Date":
                value.render = renderDateTime;
                break;
            case "Currency":
                value.render = renderCurrency;
                break;
            case "Decimal2Places":
                value.render = renderDecimal2Places;
                break;
            case "Decimal4Places":
                value.render = renderDecimal4Places;
                break;
        }
    }
    function RenderButtons(value) {
        enabledButtons.push(window[value]);
    }
    var tableName = "#<%=TableName%>";
    var fileName = "<%=FileName%>";
    var ColumnVisibility = {
        extend: 'collection',
        text: '<i class="fa fa-eye-slash" id="colVisBtn"></i> nascondi',
        buttons: ['columnsVisibility'],
        visibility: false
    };
    var Excel = {
        extend: 'excelHtml5',
        text: '<i class="fa fa-file-excel-o"></i> excel',
        title: fileName,
        exportOptions: {
            columns: ':visible:not(.noPrint)'
        },
        customize: function (xlsx) {
            var sheet = xlsx.xl.worksheets['Sheet1.xml'];
            $('row c[r^="C"]', sheet).attr('s', '2');
        }
    };
    var Pdf = {
        extend: 'pdf',
        text: '<i class="fa fa-file-pdf-o"></i> PDF',
        title: fileName,
        exportOptions: {
            columns: ':visible:not(.noPrint)'
        }
    };
    var Print = {
        extend: 'print',
        text: '<i class="fa fa-print"></i> Stampa',
        title: fileName,
        exportOptions: {
            columns: ':visible:not(.noPrint)'
        }
    };
    var enabledButtons = [];

    function ShowSmartGrid(Buttons, customDom, Objects, Columns, ColumnsDefs) {
        Columns.forEach(RenderColumns);
        ColumnsDefs.forEach(RenderColumnsDefs);
        if (Buttons == undefined) {
            enabledButtons = [];
        } else {
            Buttons.forEach(RenderButtons);
        }

        var table = $(tableName).DataTable({
            aaData: Objects,
            stateSave: false,
            bAutoWidth: false,
            fixedHeader: true,
            sDom: customDom,
            "aLengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Tutte"]],
            order: [0, "asc"],
            "aoColumns": Columns,
            "aoColumnDefs": ColumnsDefs,
            pagingType: "full_numbers",
            buttons: {
                buttons: enabledButtons,
                dom: {
                    container: {
                        className: 'dt-buttons text-left'
                    },
                    button: {
                        className: 'btn btn-default'
                    }
                }
            },
            language: {
                "emptyTable": "Non sono presenti righe.",
                "info": "Righe da _START_ a _END_ di _TOTAL_",
                "infoEmpty": "0 righe",
                "infoFiltered": "(_MAX_ totali)",
                "lengthMenu": "Mostra _MENU_",
                "iDisplayLength": 10,
                "search": "Filtra:",
                "bDestroy": true,
                "zeroRecords": "Non sono presenti righe",
                "paginate": {
                    "first": "Prima",
                    "last": "Ultima",
                    "next": "Succ.",
                    "previous": "Prec."
                },
                decimal: ",",
                thousands: "."
            },
            initComplete: function () {
                this.api().columns().every(function () {
                    var column = this;
                    if ($(column.footer()).hasClass("IncludiFiltroSelect")) {
                        var select = $('<select class="form-control auto" id="ddl' + $(this).attr('class') + '"><option value="" class="defaultOption">Seleziona</option></select>').appendTo($(column.footer()).empty()).on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );
                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });
                        column.data().unique().sort().each(function (d, j) {
                            select.append('<option value="' + d + '">' + d + '</option>')
                        });
                    }
                });
            },
            "fnDrawCallback": function (oSettings) {
                clearCheckBox();
                if ($(tableName + ' tbody tr').length >= $(tableName).DataTable().rows({ "filter": "applied" }).nodes().length) {
                    $('.dataTables_paginate').hide(); //Hide
                }
                else {
                    $('.dataTables_paginate').fadeIn("slow"); //Show
                }
            }
        });

        // Setup - add a text input to each footer cell
        $(tableName + ' tfoot th:not(.EscludiFiltro):not(.IncludiFiltroSelect)').each(function () {
            var title = $(this).data("title");
            $(this).html('<input type="text" class="form-control auto text-uppercase" placeholder="Cerca ' + title + '" id="txt' + $(this).attr('class') + '" />');
        });

        // Restore state - Footer
        var state = table.state.loaded();
        if (state) {
            table.columns().eq(0).each(function (colIdx) {
                var colSearch = state.columns[colIdx].search;

                if (colSearch.search) {
                    if ($(table.column(colIdx).footer()).hasClass("IncludiFiltroSelect")) {
                        $(table.column(colIdx).footer()).children("select").val(colSearch.search.replace('^', '').replace('$', ''));
                    }
                    else if ($(table.column(colIdx).footer()).children().is('input[type=button]')) {

                    }
                    else {
                        $('input[type=text]', table.column(colIdx).footer()).val(colSearch.search);
                    }
                }
            });

            table.draw(false);
        }

        // Apply the search
        table.columns().every(function () {
            var that = this;

            $('input', this.footer()).on('keyup change', function () {
                if (that.search() !== this.value) {
                    that
                .search(this.value)
                .draw();
                }
            });
        });
    }

    function LoadSmartGrid() {
        $.ajax({
            type: "POST",
            url: "<%=PageName%>.aspx/GetObjects",
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            //beforeSend: function () {
            //    $.blockUI({ css: { border: '0px solid white' }, message: $('#divLoad') });
            //},
            success: function (data) {
                if (data.d == null) {
                    return;
                }
                ShowSmartGrid(JSON.parse(data.d.Buttons), data.d.sDom, JSON.parse(data.d.Objects), JSON.parse(data.d.Columns), JSON.parse(data.d.ColumnsDefs));
                //hideBlockUI();
            },
            error: function (data) {
                alert(data.responseText);
                //$("#lblErroreDettaglio").show();
                //if (data.responseText != null && data.responseText != "")
                //    $("#lblErroreDettaglio").text(data.responseText);
            }
        });
    }

    // Returns selected checkbox's values
    function getSelectedCheckBox() {
        var checkedValues = [];
        var checkedCheckBox = $('.chkSmartGrid', $(tableName).DataTable().rows('.success').nodes().to$());
        if (checkedCheckBox.length == 0)
            return checkedValues;
        $.each(checkedCheckBox, function (index, chk) {
            // Create a hidden element 
            checkedValues.push(chk.value);
        });
        return checkedValues;
    }

    function setRowState(check) {
        if ($(check).parents('tr').hasClass('success')) {
            $(check).parents('tr').removeClass('success')
        }
        else {
            $(check).parents('tr').addClass('success')
        }
        clearCheckBox();
    }

    function clearCheckBox() {
        var visibleCheck = $(tableName + ' .chkSmartGrid');
        var visibleActiveCheck = $(tableName + ' tr.success .chkSmartGrid');

        $(tableName + ' thead #select_all')[0].checked = (visibleCheck.length > 0 && visibleActiveCheck.length == visibleCheck.length);

        checkSelectedRows();
    }

    function checkSelectedRows() {
        if ($(tableName + ' thead #select_all').length > 0) {
            var numOfSelectedRows = $(tableName).DataTable().rows('.success').nodes().length;
            var badgeSelectedRows = '<h5 id="bdgSelectedCount"';
            if (numOfSelectedRows > 0)
                badgeSelectedRows += ' title="Clicca per deselezionare tutte le righe" style="cursor:pointer"><span class="label label-warning">';
            else
                badgeSelectedRows += '><span class="label label-primary">';
            badgeSelectedRows += 'Righe selezionate: ' + numOfSelectedRows + '</span></h5>';

            $('.dataTables_selectedRows').html(badgeSelectedRows);

            if (numOfSelectedRows == 0) {
                $('#btnMultipleAction').addClass('disabled');
            }
            else {
                $('#btnMultipleAction').removeClass('disabled');
            }

            $('#bdgSelectedCount').on('click', function (e) {
                checkUnchecked(true);
                $(tableName).DataTable().rows('.success').nodes().to$().removeClass('success');
                clearCheckBox(true);
            });
        }
    }

    function checkUnchecked(allRows, selectAll) {
        var unchecked;
        var checked;
        if (allRows) {
            unchecked = $('td:has(.chkSmartGrid) input:not(:checked)', $(tableName).DataTable().rows('.success').nodes().to$());
            checked = $('td:has(.chkSmartGrid) input:checked', $(tableName).DataTable().rows('.success').nodes().to$());
        }
        else if (selectAll != undefined) {
            if (selectAll) {
                unchecked = $(tableName + ' tr:not(.success) .chkSmartGrid:not(:checked)');
                checked = $(tableName + ' tr:not(.success) .chkSmartGrid:checked');
            }
            else {
                unchecked = $(tableName + ' tr.success .chkSmartGrid:not(:checked)');
                checked = $(tableName + ' tr.success .chkSmartGrid:checked');
            }
        }
        else {
            unchecked = $(tableName + ' .chkSmartGrid:not(:checked)');
            checked = $(tableName + ' .chkSmartGrid:checked');
        }
        $(checked).prop('checked', false);
        $(unchecked).prop('checked', true);
    }

    $(function () {
        LoadSmartGrid();

        // Handle click on "Select all" control
        $(tableName + ' thead #select_all').on('click', function (e) {
            checkUnchecked(false, this.checked);
            if (this.checked) {
                $(tableName + ' .chkSmartGrid').parents('tr').addClass('success');
            } else {
                $(tableName + ' .chkSmartGrid').parents('tr').removeClass('success');
            }
            clearCheckBox();
            // Prevent click event from propagating to parent
            e.stopPropagation();
        });

        $(tableName).DataTable().cells(":has(input[type='checkbox'])").nodes().to$().css('text-align', 'center');

        $(tableName + ' tbody').on('click', 'td.' + detailClass, function () { doPostBackOperation(this, '<%= this.DetailKey %>'); });
        $(tableName + ' tbody').on('click', 'td.' + editClass, function () { doPostBackOperation(this, '<%= this.EditKey %>'); });
        $(tableName + ' tbody').on('click', 'td.' + deleteClass, function () { doPostBackOperation(this, '<%= this.DeleteKey %>'); });
        // Handle click on "btnMultipleAction" control
        $(tableName + ' tfoot #btnMultipleAction').on('click', function (e) {
            if (getSelectedCheckBox().length == 0)
                return;
            doPostBackOperation(undefined, '<%= this.MultipleActionKey %>');
        });

        function doPostBackOperation(Elem, Operation) {
            var paramInputOperation;
            if (Elem != undefined) {
                var dataRowKey = '<%= this.DataRowKey %>';
                var td = $(Elem).closest('td');
                var tr = $(Elem).closest('tr');

                paramInputOperation = $(td).data(dataRowKey.toLowerCase());
            }
            else { // Multiple Action
                paramInputOperation = getSelectedCheckBox().join(',');
            }
            var row = $(tableName).DataTable().row(tr);
            if (row.data() != undefined) {
                var btnID = '[id$="btnRowSelected"]';
                __doPostBack(btnID, 'Operation_' + Operation + '_' + paramInputOperation);
            }
        }

        var btnClearFilters = '<input class="btn btn-warning" id="btnClearFilters" onclick="clearFilters()" type="button" value="PULISCI FILTRI">';
        $('.dataTables_clearFilters').html(btnClearFilters);

        $('.bootStrapTarget [title]').tooltip({
            container: '.bootStrapTarget'
        });

        var toggleCheckboxes = '<input id="chkToggle" type="checkbox"/>';
        $('.dataTables_toggleCheckboxes').html(toggleCheckboxes);
        $('#chkToggle').bootstrapToggle({
            on: '<i class="fa fa-check"></i> FILTRATI',
            off: 'TUTTI',
            width: '120'
        });
        $('#chkToggle').change(function () {
            FiltraCheckBoxes(this);
        });

        var columnHidden = false;
        $('#colVisBtn').parents('a').on('click', function (e) {
            if (columnHidden)
                return;
            $('.dropdown-menu .buttons-columnVisibility :empty').hide();
            columnHidden = true;
        });
    });

    function FiltraCheckBoxes(toggle) {
        var table = $(tableName).DataTable();
        $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            if (!toggle.checked)
                return true;
            return $('.chkSmartGrid:checked', table.row(dataIndex).nodes()).length > 0;
        });
        //Update table
        table.draw();
        //Deleting the filtering function if we need the original table later.
        //$.fn.dataTable.ext.search.pop();
    }

    function clearFilters() {
        $('.dataTables_filter input[type=text]').val("");
        $('tfoot tr th').each(function () {
            $('select', this).eq(0).find('.defaultOption').attr('selected', 'selected');
            $('input[type=text]', this).val("");
        });
        //$('.chkSmartGrid', $(tableName).DataTable().rows().nodes().to$()).parents('tr').show();
        $(tableName).DataTable().search('').columns().search('').draw();
    }
</script>

<div class="table-responsive container-fluid" id="divSmartGrid" runat="server">
</div>
<asp:Button ID="btnRowSelected" runat="server" Visible="false" />