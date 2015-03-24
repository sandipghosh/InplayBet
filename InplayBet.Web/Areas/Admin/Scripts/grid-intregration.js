/// <reference path="../../../Scripts/jquery-2.1.0-vsdoc.js" />
/// <reference path="../../../Scripts/jquery-2.1.0.min.js" />
/// <reference path="../../../Scripts/common-script.js" />

/// <reference path="jquery.jqGrid.min.js" />
/// <reference path="jquery.linq-vsdoc.js" />

(function ($, win) {
    var lastSelId;

    this.defaultSubGridOptions = {
        "plusicon": "ui-icon-triangle-1-e",
        "minusicon": "ui-icon-triangle-1-s",
        "openicon": "ui-icon-arrowreturn-1-e",
        // load the subgrid data only once
        // and the just show/hide
        "reloadOnExpand": false,
        // select the row when the expand column is clicked
        "selectOnExpand": true
    };

    this.subGridRowExpandedHandler = function (options) {
        try {
            var defaultOptions = {
                renderURL: '',
                keyFieldName: '',
                modelData: null,
                parentGridElement: null,
                recordtext: 'Total Records {2}',
            };

            var obj = $.extend(defaultOptions, options);

            var functionHandler = {
                options: obj,
                handler: function (subgrid_id, row_id, obj) {
                    var subgrid_table_id, pager_id;
                    var keyValue = options.parentGridElement.jqGrid('getCell', row_id, options.keyFieldName);

                    subgrid_table_id = subgrid_id + "_t";
                    pager_id = 'p_{0}'.format(subgrid_table_id);
                    $('#{0}'.format(subgrid_id)).html('<table id="{0}" class="scroll"></table><div id="{1}" class="scroll"></div>'.format(subgrid_table_id, pager_id));

                    $('#{0}'.format(subgrid_table_id)).SetupGrid({
                        modelSchema: options.modelData,
                        datatype: 'json',
                        pagerid: pager_id,
                        recordtext: options.recordtext,
                        renderURL: '{0}?{1}={2}'.format(options.renderURL, options.keyFieldName, keyValue)
                        //editURL: '{0}/Admin/TestimonialManager/SetTestimonial'.format(virtualDirectory),
                        //editable: true,
                        //addCommandTitle: 'Add Testimonial',
                        //insert_func: function () {
                        //    insertGridRow();
                        //}
                    });
                }
            };

            return functionHandler;
        } catch (ex) {

        }
    };

    this.gridRowMode = {
        Edit: 'edit',
        Normal: 'normal'
    };

    this.OperationMode = {
        Insert: 'insert',
        Edit: 'edit',
        Delete: 'delete'
    };

    this.GetNewRowID = function () {
        try {
            var dDate = new Date()
            var rowid = parseInt(Date.UTC(dDate.getUTCFullYear(),
            dDate.getUTCMonth(), dDate.getUTCDate(), dDate.getUTCHours(),
            dDate.getUTCMinutes(), dDate.getUTCSeconds(), dDate.getUTCMilliseconds()) / 1000);
            return rowid;
        }
        catch (ex) { }
    }

    /*Implementing jqGrid setup*/
    $.fn.SetupGrid = function (options) {
        try {
            var $gridElement = $(this);
            var defaultOptions = {
                data: [],
                datatype: 'json',
                height: '100%',
                //height:'200',
                //height: resize(),
                insertMode: 'internal',
                renderURL: null,
                editURL: null,
                mtype: 'GET',
                postData: {},
                pagerid: '',
                topPager: true,
                sortname: 'id',
                //rowNum: -1,
                rowNum: 30,
                treeGrid: false,
                loadonce: false,
                prmNames: {},
                treeGridModel: null,
                ExpandColumn: false,
                viewrecords: false,
                hoverrows: false,
                gridview: true,
                scroll: 0,
                grouping: false,
                groupingView: null,
                sortorder: 'desc',
                recordtext: 'Total Records {2}',
                recordpos: 'left',
                caption: null,
                autowidth: true,
                shrinkToFit: true,
                editable: false,
                searchOperators: false,
                showHistry: false,
                subGrid: false,
                subGridOptions: null,
                subGridRowExpanded: null,
                exportCallback_Func: null,
                afterRestore_Func: null,
                gridComplete_Func: null
            };

            options.modelSchema.colModel = SetEditableHiddenColumnModel(options);
            options.modelSchema.colModel = SetFunctionReferanceIntoColumnModel(options.modelSchema.colModel);

            var obj = $.extend(defaultOptions, options);

            //set route value for renderURL
            if (obj.URIData != undefined) {
                obj.renderURL = JsonToQueryString(obj.renderURL, obj.URIData);
            }

            //set route value for editURL
            if (obj.EditURIData != undefined) {
                obj.editURL = JsonToQueryString(obj.editURL, obj.URIData);
            }

            //evaluate edit rule custom function
            $.each(obj.modelSchema.colModel, function (key, val) {
                if (val.editrules) {
                    if (val.editrules.custom_func) {
                        val.editrules.custom_func = eval(val.editrules.custom_func);
                    }
                }
            });

            $gridElement.jqGrid('GridUnload').jqGrid('GridDestroy');
            //$gridElement.jqGrid('GridDestroy');

            $gridElement.jqGrid({
                jsonReader: obj.modelSchema.jsonReader,
                url: obj.renderURL,
                data: obj.data,
                datatype: obj.datatype,
                postData: obj.postData,
                mtype: obj.mtype,
                height: obj.height,
                colNames: (obj.modelSchema.colNames == undefined ?
                    GetGridColumnNames(obj.modelSchema.colModel) : obj.modelSchema.colNames),
                colModel: obj.modelSchema.colModel,
                onSelectRow: function (id) {
                    if (id && id !== lastSelId && lastSelId != undefined) {
                        $gridElement.jqGrid('restoreRow', lastSelId, function (rowid) {
                            if (obj.afterRestore_Func != null)
                                obj.afterRestore_Func(rowid)

                            $gridElement.changeMode({ rowid: lastSelId, mode: gridRowMode.Normal });
                        });
                    }
                    lastSelId = id;
                },
                pager: obj.pagerid,
                toppager: obj.topPager,
                sortname: obj.sortname,
                rowNum: obj.rowNum,
                rowList: [20, 30, 50],
                //rowList: [2, 3, 5],
                rownumbers: true,
                rownumWidth: 20,
                treeGrid: obj.treeGrid,
                loadonce: obj.loadonce,
                prmNames: obj.prmNames,
                treeGridModel: obj.treeGridModel,
                ExpandColumn: obj.ExpandColumn,
                viewrecords: obj.viewrecords,
                hoverrows: obj.hoverrows,
                gridview: obj.gridview,
                scroll: obj.scroll,
                grouping: obj.grouping,
                groupingView: obj.modelSchema.groupingView,
                recordtext: obj.recordtext,
                //recordpos: obj.recordpos,
                //pgbuttons: false,
                //pgtext: false,
                //pginput: false,
                viewrecords: true,
                sortorder: obj.sortorder,
                editurl: obj.editURL,
                caption: obj.caption,
                autowidth: obj.autowidth,
                shrinkToFit: obj.shrinkToFit,
                altRows: true,
                altclass: 'even-bg',
                subGrid: obj.subGrid,
                subGridOptions: obj.subGridOptions,
                subGridRowExpanded: obj.subGridRowExpanded,

                //width: ($(this).parent().width() - 500),
                gridComplete: function () {
                    if (obj.editable == true) {
                        var ids = $gridElement.jqGrid('getDataIDs');
                        for (var i = 0; i < ids.length; i++) {
                            $gridElement.changeMode({ rowid: ids[i], mode: gridRowMode.Normal });
                        }
                    }
                    if (obj.gridComplete_Func != null)
                        obj.gridComplete_Func();
                }
            }).setGridWidth($(this).parent().width());

            /*Configure grid navigation panel -- disable all command 
            except refresh and enable cloneToTop property*/
            if (obj.pagerid != '') {
                $gridElement.jqGrid('navGrid', obj.pagerid,
                {
                    edit: false,
                    add: false,
                    del: false,
                    search: false,
                    refresh: false,
                    cloneToTop: true,
                    savekey: [false]
                });
            }

            if (obj.searchOperators) {
                $gridElement.jqGrid('filterToolbar', { searchOperators: obj.searchOperators, stringResult: true, defaultSearch: 'cn', ignoreCase: true });
            }

            SetAddButtonOnTopAndButtom($gridElement, obj);

            //Removing title bar close button
            $('.ui-jqgrid-titlebar-close').remove();

        } catch (ex) {
            if (win.console)
                win.console.log(ex);
        }
    };

    $.fn.changeMode = function (options) {
        try {
            var $gridElement = $(this);
            var defaultOptions = {
                mode: 'normal',
                rowid: '',
                editFunc: 'editGridRow',
                deleteFunc: null,
                saveFunc: 'saveGridRow',
                restoreFunc: 'restoreGridRow'
            },
            $editCommand, $deleteCommand, $saveCommand, $cancelCommand;
            var obj = $.extend(defaultOptions, options);

            $editCommand = $("<span class=\"grid-command command_edit\" title=\"Edit\" onclick=\"{0}('{1}')\"></span>".format(obj.editFunc, obj.rowid));
            $deleteCommand = $("<span class=\"grid-command command_delete\" title=\"Delete\" onclick=\"{0}('{1}')\"></span>".format(obj.deleteFunc, obj.rowid));
            $saveCommand = $("<span class=\"grid-command command_save\" title=\"Save\" onclick=\"{0}('{1}')\"></span>".format(obj.saveFunc, obj.rowid));
            $cancelCommand = $("<span class=\"grid-command command_cancel\" title=\"Cancel\" onclick=\"{0}('{1}')\"></span>".format(obj.restoreFunc, obj.rowid));

            if (obj.mode == gridRowMode.Normal) {
                $gridElement.SwitchClass('edit-mode', 'normal-mode');
                $gridElement.jqGrid('setRowData', obj.rowid, {
                    Actions: $editCommand[0].outerHTML //+ $deleteCommand[0].outerHTML
                }, 'normal-mode');
            }
            else if (obj.mode == gridRowMode.Edit) {
                $gridElement.SwitchClass('normal-mode', 'edit-mode');
                $gridElement.jqGrid('setRowData', obj.rowid, {
                    Actions: $saveCommand[0].outerHTML + $cancelCommand[0].outerHTML
                }, 'edit-mode');
            }

            $gridElement.find('.grid-command').each(function () {
                var $self = $(this);
                $self.hover(
                    function () { $self.addClass('grid-command-hover') },
                    function () { $self.removeClass('grid-command-hover') }
                );
            });

            return $gridElement;
        }
        catch (ex) {
            if (win.console)
                win.console.log(ex);
        }
    }

    $.fn.SwitchClass = function (selectors) {
        try {
            var _selectors = [],
            _Switch = function (o, oldClass, newClass) {
                if (o && oldClass && newClass && o.hasClass(oldClass)) {
                    o.removeClass(oldClass).addClass(newClass);
                }
            };

            if ($.isArray(selectors)) { _selectors = selectors; }
            else { _selectors.push(selectors); }
            if (_selectors.length === 0) { return; }

            $.each(_selectors, function (idx, selector) {
                _Switch($(selector.elem), selector.old_class, selector.new_class);
            });
        } catch (ex) {
            if (win.console)
                win.console.log(ex);
        }
    };

    var SetEditableHiddenColumnModel = function (options) {
        try {

            $.Enumerable.From(options.modelSchema.colModel)
            .Where(function (x) {
                return (x.editable == true && x.hidden == true && x.edittype == 'custom');
            }).ForEach(function (x) {
                x.editoptions = { custom_element: InsertHiddenElement, custom_value: SetHiddenElementValue };
            });

            return options.modelSchema.colModel;

        } catch (ex) {
            console.log(ex);
        }
    };

    //Callback handler to insert hidden element into grid model to track changes
    var InsertHiddenElement = function (value, option) {
        return $('<input type="hidden" name="{0}" value="{1}" />'.format(option.name, value));
    };

    //Callback handler to return change status
    var SetHiddenElementValue = function (elem) {
        return elem.val();
    };

    /*This function is responsible to evaluate function name to function handler specified in grid column model*/
    var SetFunctionReferanceIntoColumnModel = function (modelSchema) {
        try {
            $.Enumerable.From(modelSchema)
                .ForEach(function (outer) {
                    if (outer.formatter != undefined && outer.formatter.startsWith('fn'))
                        outer.formatter = new Function(outer.formatter.split(':')[1]);

                    if (resolveParent(outer, 'editoptions.buildSelect') != undefined)
                        if (typeof outer.editoptions.buildSelect != 'function') {
                            outer.editoptions.buildSelect = new Function(outer.editoptions.buildSelect);
                        }

                    if (resolveParent(outer, 'editoptions.dataInit') != undefined)
                        if (typeof outer.editoptions.dataInit != 'function') {
                            outer.editoptions.dataInit = eval(outer.editoptions.dataInit);
                        }

                    if (resolveParent(outer, 'editoptions.custom_element') != undefined)
                        if (typeof outer.editoptions.custom_element != 'function') {
                            outer.editoptions.custom_element = new Function(outer.editoptions.custom_element);
                        }

                    if (resolveParent(outer, 'editoptions.custom_value') != undefined)
                        if (typeof outer.editoptions.custom_value != 'function') {
                            outer.editoptions.custom_value = new Function(outer.editoptions.custom_value);
                        }

                    if (resolveParent(outer, 'editrules.custom_func') != undefined)
                        if (typeof outer.editrules.custom_func != 'function') {
                            outer.editrules.custom_func = new Function(outer.editrules.custom_func);
                        }
                });

            return modelSchema;
        } catch (ex) {
            if (win.console)
                win.console.log(ex);
        }
    };

    var JsonToQueryString = function (rawUrl, jsonData) {
        try {
            if (rawUrl == undefined || rawUrl == null)
                return null;

            var url = rawUrl + '?', i = 0;
            $.each(jsonData, function (key, val) {
                url += (i == 0 ? key : '&' + key) + '=' + val;
                i += 1;
            });

            return url;
        } catch (ex) {
            if (win.console)
                win.console.log(ex);
        }
    };

    var GetGridColumnNames = function (models) {
        try {
            var columnNames = [];
            $.each(models, function (key, val) {
                if (val.label)
                    columnNames.push(val.label);
                else
                    columnNames.push(GetLetterCaseFromPascalCase(val.name));
            });
            return columnNames;
        } catch (ex) {
            if (win.console)
                win.console.log(ex);
        }
    };

    this.initDatepickerOnDateEdit = function (elem) {
        $(elem).datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            minDate: new Date(new Date().getTime() + 24 * 60 * 60 * 1000),//new Date($('.datepicker-input').attr('minDate')),
            dateFormat: 'mm-dd-yy',
            defaultDate: new Date()
        });
    };

    var resolveParent = function (obj, path) {
        var parts = path.split(/[.]/g);
        var parent;
        for (var i = 0; i < parts.length && obj; i++) {
            var p = parts[i];
            if (p in obj) {
                parent = obj;
                obj = obj[p];
            } else {
                return undefined;
            }
        }
        return parent;
    };

    var SetAddButtonOnTopAndButtom = function ($grid, params) {
        try {
            /*Attach custom add command to the bottom and top navigation panel and associate with add handler*/
            if (params.editable == true && params.insertMode == 'internal') {
                $grid.jqGrid('navButtonAdd', params.pagerid,
                {
                    title: (params.addCommandTitle == undefined ? "Add" : params.addCommandTitle),
                    caption: (params.addCommandTitle == undefined ? "" : params.addCommandTitle),
                    buttonicon: "command_add",
                    onClickButton: (params.insert_func == undefined ? null : params.insert_func)
                });
                $grid.jqGrid('navButtonAdd', $grid.selector + '_toppager_left',
                {
                    title: (params.addCommandTitle == undefined ? "Add" : params.addCommandTitle),
                    caption: (params.addCommandTitle == undefined ? "" : params.addCommandTitle),
                    buttonicon: "command_add",
                    onClickButton: (params.insert_func == undefined ? null : params.insert_func)
                });
            }

            if (params.exportCallback_Func != null) {
                $grid.jqGrid('navButtonAdd', params.pagerid, {
                    caption: "Export to Excel",
                    buttonicon: "command_excel_export",
                    onClickButton: function () {
                        var gridData = $grid.jqGrid('getRowData');
                        var dataToSend = Base64Encode(JSON.stringify(gridData));
                        params.exportCallback_Func(dataToSend);
                    }
                });
                $grid.jqGrid('navButtonAdd', $grid.selector + '_toppager_left',
                {
                    caption: "Export to Excel",
                    buttonicon: "command_excel_export",
                    onClickButton: function () {
                        var gridData = $grid.jqGrid('getRowData');
                        var dataToSend = Base64Encode(JSON.stringify(gridData));
                        params.exportCallback_Func(dataToSend);
                    }
                });
            }

        } catch (ex) {
            console.log(ex);
        }
    }
}(jQuery, window));