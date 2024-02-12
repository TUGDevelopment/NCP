window.DevAV = (function () {
    var updateTimerID = -1;
    //var updateTimeout = 300;
    //var searchBoxTimer = -1;
    //var cardClassName = "dvCard";
    //var cardViewFocusClassName = "focusCard";
    var pendingCallbacks = {};
    var DXUploadedFilesContainer = {
        
        Clear: function () {
            DXUploadedFilesContainer.ReplaceHtml("");
        },
        AddFile: function (fileName, fileUrl, fileSize) {
            var self = DXUploadedFilesContainer;
            var builder = ["<tr>"];

            builder.push("<td class='nameCell'");
            if (self.nameCellStyle)
                builder.push(" style='" + self.nameCellStyle + "'");
            builder.push(">");
            self.BuildLink(builder, fileName, fileUrl);
            builder.push("</td>");

            builder.push("<td class='sizeCell'");
            if (self.sizeCellStyle)
                builder.push(" style='" + self.sizeCellStyle + "'");
            builder.push(">");
            builder.push(fileSize);
            builder.push("</td>");

            builder.push("</tr>");

            var html = builder.join("");
            DXUploadedFilesContainer.AddHtml(html);
        },
        BuildLink: function (builder, text, url) {
            builder.push("<a target='blank' onclick='return ShowScreenshotWindow(event, this, " + this.useExtendedPopup + ");'");
            builder.push(" href='" + url + "'>");
            builder.push(text);
            builder.push("</a>");
        },
        AddHtml: function (html) {
            var fileContainer = document.getElementById("uploadedFilesContainer"),
                fullHtml = html;
            if (fileContainer) {
                var containerBody = fileContainer.tBodies[0];
                fullHtml = containerBody.innerHTML + html;
            }
            DXUploadedFilesContainer.ReplaceHtml(fullHtml);
        },
        ReplaceHtml: function (html) {
            var builder = ["<table id='uploadedFilesContainer' class='uploadedFilesContainer'><tbody>"];
            builder.push(html);
            builder.push("</tbody></table>");
            var contentHtml = builder.join("");
            FilesRoundPanel.SetContentHtml(contentHtml);
        },
        ApplySettings: function (nameCellStyle, sizeCellStyle, useExtendedPopup) {
            var self = DXUploadedFilesContainer;
            self.nameCellStyle = nameCellStyle;
            self.sizeCellStyle = sizeCellStyle;
            self.useExtendedPopup = useExtendedPopup;
        }
    };
    ShowScreenshotWindow = function (evt, link, useExtendedPopup) {
        ShowScreenshot(link.href, useExtendedPopup);
        evt.cancelBubble = true;
        return false;
    };
    ShowScreenshot = function (src, useExtendedPopup) {
        var getPopupFunc = useExtendedPopup ? getScreenshotExtendedPopup : getScreenshotPopup;
        ShowScreenshotCore(src, getPopupFunc);
    };
    ShowScreenshotCore = function (src, getPopupFunc) {
        var screenLeft = document.all && !document.opera ? window.screenLeft : window.screenX;
        var screenWidth = screen.availWidth;
        var screenHeight = screen.availHeight;
        var zeroX = Math.floor((screenLeft < 0 ? 0 : screenLeft) / screenWidth) * screenWidth;

        var windowWidth = 475;
        var windowHeight = 325;
        var windowX = parseInt((screenWidth - windowWidth) / 2);
        var windowY = parseInt((screenHeight - windowHeight) / 2);
        if (windowX + windowWidth > screenWidth)
            windowX = 0;
        if (windowY + windowHeight > screenHeight)
            windowY = 0;

        windowX += zeroX;

        var popupwnd = getPopupFunc(src, windowX, windowY, windowWidth, windowHeight);
        if (popupwnd != null && popupwnd.document != null && popupwnd.document.body != null) {
            popupwnd.document.body.style.margin = "0px";
        }
    };
    getScreenshotPopup = function (src, windowX, windowY, windowWidth, windowHeight, showScrollbars) {
        var scrollbars = showScrollbars ? "yes" : "no";
        return window.open(src, '_blank', "left=" + windowX + ",top=" + windowY + ",width=" + windowWidth + ",height=" + windowHeight + ", scrollbars=" + scrollbars + ", resizable=no", true);
    };
    getScreenshotExtendedPopup = function (src, windowX, windowY, windowWidth, windowHeight) {
        var popup = getScreenshotPopup("", windowX, windowY, windowWidth, windowHeight, true);
        var doc = popup.document,
            img = doc.createElement("img");

        img.src = src;
        img.style.width = "100%";
        img.style.height = "100%";
        doc.body.appendChild(img);
        return popup;
    };
    //var callbackHelper = (function () {
    //    var callbackControlQueue = [],
    //        currentCallbackControl = null;
    function ChangeDemoState (view, command, key) {
        //var prev = this.DemoState;
        var current = { View: view, Command: command, Key: key };
        //if (prev && current && prev.View == current.View && prev.Command == current.Command && prev.Key == current.Key)
        //    return;
        DemoState = current;
        OnStateChanged();
        ShowMenuItems();
    };
    function getUrlVars() {
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    };
    function ShowLoadingPanel(element) {
        this.loadingPanelTimer = window.setTimeout(function () {
            LoadingPanel.ShowInElement(element);
        }, 500);
    };
    function HideLoadingPanel() {
        if (this.loadingPanelTimer > -1) {
            window.clearTimeout(this.loadingPanelTimer);
            this.loadingPanelTimer = -1;
        }
        LoadingPanel.Hide();
    };
    function DoCallback (sender, callback) {
        if (sender.InCallback()) {
            PendingCallbacks[sender.name] = callback;
            sender.EndCallback.RemoveHandler(DoEndCallback);
            sender.EndCallback.AddHandler(DoEndCallback);
        } else {
            callback();
        }
    };
    function DoEndCallback (s, e) {
        var pendingCallback = PendingCallbacks[s.name];
        if (pendingCallback) {
            pendingCallback();
            delete PendingCallbacks[s.name];
        }
    };
   function PostponeAction(action, canExecute) {
        var f = function () {
            if (!canExecute())
                window.setTimeout(f, 50);
            else
                action();
        };
        f();
    };
    //    function doCallback(callbackControl, args, sender) {
    //        if (!currentCallbackControl) {
    //            currentCallbackControl = callbackControl;
    //            if (typeof (detailsCallbackPanel) !== "undefined" && callbackControl == mainCallbackPanel)
    //                detailsCallbackPanel.cpSkipUpdateDetails = true;
    //            callbackControl.EndCallback.RemoveHandler(onEndCallback);
    //            callbackControl.EndCallback.AddHandler(onEndCallback);
    //            callbackControl.PerformCallback(args);
    //        } else
    //            placeInQueue(callbackControl, args, getSenderId(sender));
    //    };
    //    function getSenderId(senderObject) {
    //        if (senderObject.constructor === String)
    //            return senderObject;
    //        return senderObject.name || senderObject.id;
    //    };
    //    function placeInQueue(callbackControl, args, sender) {
    //        var queue = callbackControlQueue;
    //        for (var i = 0; i < queue.length; i++) {
    //            if (queue[i].control == callbackControl && queue[i].sender == sender) {
    //                queue[i].args = args;
    //                return;
    //            }
    //        }
    //        queue.push({ control: callbackControl, args: args, sender: sender });
    //    };
    //    function onEndCallback(sender) {
    //        sender.EndCallback.RemoveHandler(onEndCallback);
    //        currentCallbackControl = null;
    //        var queuedPanel = callbackControlQueue.shift();
    //        if (queuedPanel)
    //            doCallback(queuedPanel.control, queuedPanel.args, queuedPanel.sender);
    //    }
    //    return {
    //        DoCallback: doCallback
    //    };
    //})();

    //function updateDetailInfo(sender) { // TODO use one method to create timer
    //    if (detailsCallbackPanel.cpSkipUpdateDetails) {
    //        detailsCallbackPanel.cpSkipUpdateDetails = false;
    //        return;
    //    }
    //    if (updateTimerID > -1)
    //        window.clearTimeout(updateTimerID);
    //    updateTimerID = window.setTimeout(function () {
    //        window.clearTimeout(updateTimerID);
    //        callbackHelper.DoCallback(detailsCallbackPanel, "", sender);
    //    }, updateTimeout);
    //};
    //function addTask(employeeID, sender) {
    //    employeeID = employeeID ? employeeID.toString() : "";
    //    performTaskCommand("New", employeeID, sender);
    //}
    //function editTask(id, sender) {
    //    performTaskCommand("Edit", id, sender);
    //};
    //function performTaskCommand(commandName, args, sender) {
    //    showClearedPopup(taskEditPopup);
    //    callbackHelper.DoCallback(taskEditPopup, commandName + "|" + args, sender);
    //};
    //function deleteTask(id, sender) {
    //    if (checkReadOnlyMode())
    //        return;
    //    if (confirm("Remove task?"))
    //        callbackHelper.DoCallback(mainCallbackPanel, serializeArgs(["DeleteEntry", id]), sender);
    //};
    //function gridCustomizationWindow_CloseUp() {
    //    toolbarMenu.GetItemByName("ColumnsCustomization").SetChecked(false);
    //};
    //function cardView_Init(s, e) {
    //    ASPxClientUtils.AttachEventToElement(s.GetMainElement(), "click", function (evt) {
    //        var cardID = getCardID(ASPxClientUtils.GetEventSource(evt));
    //        if (cardID)
    //            selectCard(cardID, s);
    //    });
    //    if (s.cpSelectedItemID)
    //        selectCard(s.cpSelectedItemID, s);

    //    setToolbarCWItemEnabled(false);
    //};
    //function cardView_EndCallback(s, e) {
    //    if (s.cpSelectedItemID)
    //        selectCard(s.cpSelectedItemID, s);
    //};

    //function selectCard(id, sender) {
    //    var card = document.getElementById(id);
    //    if (!card || card.className.indexOf(cardViewFocusClassName) > -1)
    //        return;

    //    var prevSelectedCard = document.getElementById(hiddenField.Get("ID"));
    //    if (prevSelectedCard)
    //        prevSelectedCard.className = ASPxClientUtils.Trim(prevSelectedCard.className.replace(cardViewFocusClassName, ""));

    //    card.className += " " + cardViewFocusClassName;
    //    hiddenField.Set("ID", id);

    //    var updateDetails = page === employeePage; //TODO add flag to the page 
    //    if (updateDetails)
    //        callbackHelper.DoCallback(detailsCallbackPanel, "", sender);
    //};
    //function getCardID(element) {
    //    while (element && element.tagName !== "BODY") {
    //        if (element.className && element.className.indexOf(cardClassName) > -1)
    //            return element.id;
    //        element = element.parentNode;
    //    }
    //    return null;
    //};
    //function setToolbarCWItemEnabled(enabled) {
    //    var item = toolbarMenu.GetItemByName("ColumnsCustomization");
    //    if (!item)
    //        return;
    //    item.SetEnabled(enabled);
    //    item.SetChecked(false);
    //}

    //function employeeSaveButton_Click(s, e) {
    //    var commandName = employeeEditPopup.cpEmployeeID ? "Edit" : "New";
    //    saveEditForm(employeeEditPopup, serializeArgs([commandName, employeeEditPopup.cpEmployeeID]));
    //};
    //function employeeCancelButton_Click(s, e) {
    //    employeeEditPopup.Hide();
    //};
    //function evaluationSaveButton_Click(s, e) {
    //    saveEditForm(evaluationEditPopup, serializeArgs([evaluationEditPopup.cpEvaluationID]), true);
    //};
    //function evaluationCancelButton_Click(s, e) {
    //    evaluationEditPopup.Hide();
    //};
    //function taskSaveButton_Click(s, e) {
    //    var commandName = taskEditPopup.cpTaskID ? "Edit" : "New";
    //    saveEditForm(taskEditPopup, serializeArgs([commandName, taskEditPopup.cpTaskID]), page === employeePage);
    //};
    //function taskCancelButton_Click(s, e) {
    //    taskEditPopup.Hide();
    //};
    function gridData_RowDblClick(s, e) {
        var key = s.GetRowKey(e.visibleIndex);
        //alert("test :" + key);
        //txtBooksNo.SetText(key);

        showClearedPopup(transferEditPopup);
        transferEditPopup.PerformCallback('read|' + key);
    };
    function xxxCancelButton_Click(s, e) {
        transferEditPopup.Hide();
    };
    function xxxSaveButton_Click(s, e) {
        //var commandName = taskEditPopup.cpTaskID ? "Edit" : "New";
        //saveEditForm(taskEditPopup, serializeArgs([commandName, taskEditPopup.cpTaskID]), page === employeePage);
        //alert("save");
        gridData.PerformCallback('save|0');
        transferEditPopup.Hide();
    };
    function test(s, e) {
        //debugger;
        //alert("test");
        //var commandName = taskEditPopup.cpTaskID ? "Edit" : "New";
        //saveEditForm(transferEditPopup, serializeArgs(["New", "0"]),tu);
        //callbackHelper.DoCallback(transferEditPopup, "New|" + args, sender);
        //testgrid.PerformCallback('AddRow');
        page.test(s, e);
    };
    function ClearForm(value) {
        page.ClearForm(value);
    };
    function OnStateChanged(s, e) {
        page.OnStateChanged(s, e);
    };
    function ShowMenuItems(s, e) {
        page.ShowMenuItems(s, e);
    };
    function currentdate() {
        //debugger;
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!
        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        var today = dd + '-' + mm + '-' + yyyy;
        return today;
    };
    function getparam() {
        var value = "param";
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi,
            function (m, key, value) {
                vars[key] = value;
            });
        if (vars[value] != undefined) {
            return vars[value];
        } else return null;
    };
    function SetUnitPriceColumnVisibility() {
        //debugger;
        var visible = false;
        var disp = visible ? 'table-cell' : 'none';
        $('td.unitPriceColumn').css('display', disp);
    };
    //function customerSaveButton_Click(s, e) { // TODO rename CustomerEmployeeForm(Button)_Click
    //    saveEditForm(customerEmployeeEditPopup, serializeArgs([customerEmployeeEditPopup.cpCustomerEmployeeID]), true);
    //};
    //function customerCancelButton_Click(s, e) {
    //    customerEmployeeEditPopup.Hide();
    //};
    //function revenueAnalysisCloseButton_Click(s, e) {
    //    revenueAnalysisPopup.Hide();
    //};

    //function getViewModeCore(key) {
    //    return ASPxClientUtils.GetCookie(key);
    //};
    //function setViewModeCore(key, value) {
    //    ASPxClientUtils.SetCookie(key, value);
    //};
    //function showEditMessagePopup(messageTemplate, operation) {
    //    var message = messageTemplate.replace("<<Operation>>", operation);
    //    editMessagePopup.SetContentHtml(message);
    //    editMessagePopup.Show();
    //};
    //function checkReadOnlyMode() {
    //    if (window.readOnlyPopup) { // TODO use hiddenField and one popupControl to readOnly and edit message
    //        readOnlyPopup.Show();
    //        return true;
    //    }
    //    return false;
    //};
    function showClearedPopup(popup) {
        popup.Show();
        ASPxClientEdit.ClearEditorsInContainer(document.getElementById("EditFormsContainer"));
    };
    function FilesContainer(s) {
        DXUploadedFilesContainer.ApplySettings(s);
    };
    function Clear() {
        DXUploadedFilesContainer.Clear();
    };
    function AddFile(fileName, fileUrl, fileSize) {
        DXUploadedFilesContainer.AddFile(fileName, fileUrl, fileSize);
    }
    //function getAttribute(element, attrName) {
    //    if (element.getAttribute)
    //        return element.getAttribute(attrName);
    //    else if (element.getProperty)
    //        return element.getProperty(attrName);
    //};

    //function saveEditForm(popup, args, isDetail) {
    //    if (!ASPxClientEdit.ValidateEditorsInContainer(popup.GetMainElement()))
    //        return;
    //    popup.Hide();
    //    if (checkReadOnlyMode())
    //        return;
    //    var callbackArgs = ["SaveEditForm", popup.cpEditFormName, args];
    //    var panel = isDetail ? detailsCallbackPanel : mainCallbackPanel;
    //    callbackHelper.DoCallback(panel, serializeArgs(callbackArgs), popup);
    //};

    //function showPivotGrid() {
    //    revenueAnalysisPopup.SetContentUrl("PivotGrid.aspx");
    //    revenueAnalysisPopup.Show();
    //};

    //function openReport(reportName, itemID) {
    //    var url = "DocumentViewer.aspx?ReportArgs=" + serializeArgs([reportName, itemID]);
    //    openPageViewerPopup(url, reportName);
    //};
    //function openSpreadsheet(reportName, itemID) {
    //    var url = "Spreadsheet.aspx?ReportArgs=" + serializeArgs([reportName, itemID]);
    //    openPageViewerPopup(url, reportName);
    //};
    //function openPageViewerPopup(contentUrl, reportName) {
    //    pageViewerPopup.SetHeaderText(pageViewerPopup.cpReportDisplayNames[reportName]);
    //    pageViewerPopup.Show();
    //    pageViewerPopup.SetContentUrl(contentUrl);
    //};

    //var dashboardPage = (function () {
    //    function toolbarMenu_ItemClick(s, e) {
    //        switch (e.item.name) {
    //            case "PivotGrid":
    //            case "RevenueChart":
    //            case "OpportunitiesChart":
    //                var panel = dockManager.GetPanelByUID(e.item.name);
    //                if (e.item.GetChecked())
    //                    panel.Show()
    //                else
    //                    panel.Hide();
    //        }
    //        alert("testxxx:" + e.item.name);
    //    }

    //    function pivotGridPanel_CloseUp(s, e) {
    //        uncheckToolbarMenuItem(s.panelUID);
    //    }
    //    function revenueChartPanel_CloseUp(s, e) {
    //        uncheckToolbarMenuItem(s.panelUID);
    //    }
    //    function opportunitiesChartPanel_CloseUp(s, e) {
    //        uncheckToolbarMenuItem(s.panelUID);
    //    }
    //    function uncheckToolbarMenuItem(itemName) {
    //        toolbarMenu.GetItemByName(itemName).SetChecked(false);
    //    }

    //    function dockManager_Init(s, e) {
    //        revenueChart.GetMainElement().style.width = "100%"; // TODO check this case
    //        opportunitiesChart.GetMainElement().style.width = "100%";
    //        tileLayoutHelper.DockManager_Init(s, e);
    //    };
    //    function dockManager_StartPanelDragging(s, e) {
    //        tileLayoutHelper.DockManager_StartPanelDragging(s, e);
    //    }
    //    function dockManager_EndPanelDragging(s, e) {
    //        tileLayoutHelper.DockManager_EndPanelDragging(s, e);
    //    }
    //    function dockManager_AfterDock(s, e) {
    //        tileLayoutHelper.DockManager_AfterDock(s, e);
    //    }
    //    function dockManager_PanelCloseUp(s, e) {
    //        tileLayoutHelper.DockManager_PanelCloseUp(s, e);
    //    }

    //    var tileLayoutHelper = (function () {
    //        var zones = [];
    //        var zoneDimensionsCache = [];
    //        var draggingPanel;

    //        function dockManager_Init(s, e) {
    //            zones = dockManager.GetZones();
    //            adjustPanes();
    //            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(onWindowResize);
    //        }
    //        function dockManager_StartPanelDragging(s, e) {
    //            draggingPanel = e.panel;
    //            ASPxClientUtils.AttachEventToElement(document, "mousemove", onMouseMove);
    //        }
    //        function dockManager_EndPanelDragging(s, e) {
    //            if (!e.panel.GetOwnerZone())
    //                dockPanelToVacantZone(e.panel);
    //            ASPxClientUtils.DetachEventFromElement(document, "mousemove", onMouseMove);
    //            draggingPanel = null;
    //        }
    //        function dockManager_AfterDock(s, e) {
    //            var zone = e.panel.GetOwnerZone();
    //            if (zone.GetPanelCount() > 1) {
    //                dockPanelToVacantZone(zone.GetPanels()[0]);
    //                zone.GetMainElement().style.height = e.panel.GetMainElement().offsetHeight + "px";
    //            }
    //            adjustPanes();
    //            adjustTopDockZone();
    //        }
    //        function dockManager_PanelCloseUp(s, e) {
    //            adjustTopDockZone();
    //        }

    //        function adjustTopDockZone() {
    //            var zone = dockManager.GetZoneByUID("TopDockZone");
    //            if (!zone.GetPanelByVisibleIndex(0).GetVisible())
    //                zone.GetMainElement().style.height = "1px";
    //        }
    //        function onWindowResize() {
    //            window.clearTimeout(updateTimerID);
    //            updateTimerID = window.setTimeout(adjustPanes, updateTimeout);
    //        }
    //        function adjustPanes() {
    //            window.clearTimeout(updateTimerID);
    //            adjustChartSize(revenueChart);
    //            adjustChartSize(opportunitiesChart);
    //            updateZoneDimensionsCache();
    //        }
    //        function adjustChartSize(chart) {
    //            var mainElement = chart.GetMainElement();
    //            var img = mainElement.getElementsByTagName("IMG")[0];
    //            var chartWidth = mainElement.offsetWidth;
    //            var imgWidth = img.offsetWidth;
    //            if (imgWidth < chartWidth && imgWidth < 500 || imgWidth > chartWidth)
    //                callbackHelper.DoCallback(chart, chartWidth, chart);
    //        };
    //        function onMouseMove(e) {
    //            var zone = getZoneUnderCursor(e);
    //            if (!zone || !draggingPanel)
    //                return;
    //            var zonePanel = zone.GetPanelCount() > 0 && zone.GetPanels()[0];
    //            if (!zonePanel || zonePanel.panelUID === draggingPanel.panelUID)
    //                return;
    //            dockPanelToVacantZone(zonePanel, zone);
    //            zone.ShowPanelPlaceholder(draggingPanel);
    //        }

    //        function dockPanelToVacantZone(panel, overredZone) {
    //            var vacantZone = getVacantZone(overredZone);
    //            panel.Dock(vacantZone);
    //            panel.GetMainElement().style.width = panel.GetMainElement().parentNode.offsetWidth;
    //        };
    //        function getZoneUnderCursor(e) {
    //            var evtX = ASPxClientUtils.GetEventX(e),
    //                evtY = ASPxClientUtils.GetEventY(e);
    //            for (var i = 0; i < zoneDimensionsCache.length; i++) {
    //                var zd = zoneDimensionsCache[i];
    //                if (evtX > zd.x && evtX < zd.x + zd.w && evtY > zd.y && evtY < zd.y + zd.h)
    //                    return dockManager.GetZoneByUID(zd.zoneUID);
    //            }
    //            return null;
    //        };
    //        function getVacantZone(excludeZone) {
    //            for (var i = 0; i < zones.length; i++) {
    //                var isExcludedZone = excludeZone && (zones[i].zoneUID === excludeZone.zoneUID);
    //                if (!isExcludedZone && zones[i].GetPanelCount() === 0)
    //                    return zones[i];
    //            }
    //        };
    //        function updateZoneDimensionsCache() {
    //            zoneDimensionsCache = [];
    //            for (var i = 0; i < zones.length; i++) {
    //                var zoneElem = zones[i].GetMainElement();
    //                zoneDimensionsCache.push({
    //                    zoneUID: zones[i].zoneUID,
    //                    x: ASPxClientUtils.GetAbsoluteX(zoneElem),
    //                    y: ASPxClientUtils.GetAbsoluteY(zoneElem),
    //                    w: zones[i].GetWidth(),
    //                    h: zones[i].GetHeight()
    //                });
    //            }
    //        };
    //        return {
    //            DockManager_Init: dockManager_Init,
    //            DockManager_StartPanelDragging: dockManager_StartPanelDragging,
    //            DockManager_EndPanelDragging: dockManager_EndPanelDragging,
    //            DockManager_AfterDock: dockManager_AfterDock,
    //            DockManager_PanelCloseUp: dockManager_PanelCloseUp
    //        };
    //    })();

    //    return {
    //        DockManager_Init: dockManager_Init,
    //        DockManager_StartPanelDragging: dockManager_StartPanelDragging,
    //        DockManager_EndPanelDragging: dockManager_EndPanelDragging,
    //        DockManager_AfterDock: dockManager_AfterDock,
    //        DockManager_PanelCloseUp: dockManager_PanelCloseUp,
    //        PivotGridPanel_CloseUp: pivotGridPanel_CloseUp,
    //        RevenueChartPanel_CloseUp: revenueChartPanel_CloseUp,
    //        OpportunitiesChartPanel_CloseUp: opportunitiesChartPanel_CloseUp,
    //        ToolbarMenu_ItemClick: toolbarMenu_ItemClick
    //    };
    //})();

    //var employeePage = (function () {
    //    function toolbarMenu_ItemClick(s, e) {
    //        var employeeID = getSelectedEmployeeID();
    //        var name = e.item.name;
    //        switch (name) {
    //            case "GridView":
    //                if (isGridViewMode())
    //                    return;
    //                setViewMode(name);
    //                callbackHelper.DoCallback(mainCallbackPanel, "", s);
    //                break;
    //            case "CardsView":
    //                if (!isGridViewMode())
    //                    return;
    //                setViewMode(name);
    //                callbackHelper.DoCallback(mainCallbackPanel, "", s);
    //                break;
    //            case "ColumnsCustomization":
    //                if (employeesGrid.IsCustomizationWindowVisible())
    //                    employeesGrid.HideCustomizationWindow();
    //                else
    //                    employeesGrid.ShowCustomizationWindow(e.htmlElement);
    //                break;
    //            case "New":
    //                addEmployee();
    //                break;
    //            case "Delete":
    //                deleteEmployee(employeeID, s);
    //                break;
    //            case "Meeting":
    //                showEditMessagePopup(editMessagePopup.cpEmployeeEditMessageTemplate, "create new meeting");
    //                break;
    //            case "Task":
    //                addTask(employeeID, s);
    //                break;
    //        }
    //    }

    //    function employeesGrid_Init(s, e) {
    //        setToolbarCWItemEnabled(true);
    //    }
    //    function employeesGrid_FocusedRowChanged(s, e) {
    //        updateDetailInfo(s);
    //    }
    //    function employeesGrid_EndCallback(s, e) {
    //        updateDetailInfo(s); // TODO check this case
    //    }
    //    function employeesGrid_ContextMenuItemClick(s, e) {
    //        switch (e.item.name) {
    //            case "NewRow":
    //                addEmployee();
    //                e.handled = true;
    //                break;
    //            case "EditRow":
    //                editEmployee(s.GetRowKey(e.elementIndex), s);
    //                e.handled = true;
    //                break;
    //            case "DeleteRow":
    //                deleteEmployee(s.GetRowKey(e.elementIndex), s);
    //                e.handled = true;
    //                break;
    //        }
    //    }

    //    function gridEditButton_Click(e) {
    //        var src = ASPxClientUtils.GetEventSource(e);
    //        editEmployee(src.id, src);
    //    };

    //    function addEmployee() {
    //        employeeEditPopup.SetHeaderText("New Employee");
    //        showClearedPopup(employeeEditPopup);
    //        firstNameTextBox.Focus();
    //    }
    //    function editEmployee(id, sender) { // TODO
    //        showClearedPopup(employeeEditPopup);
    //        callbackHelper.DoCallback(employeeEditPopup, id, sender);
    //    }
    //    function deleteEmployee(id, sender) {
    //        if (checkReadOnlyMode())
    //            return;
    //        if (confirm("Remove employee?"))
    //            callbackHelper.DoCallback(mainCallbackPanel, serializeArgs(["DeleteEntry", id]), sender);
    //    }

    //    function employeeEditButton_Click(s, e) {
    //        editEmployee(s.cpEmployeeID, s);
    //    }

    //    function evaluationGrid_CustomButtonClick(s, e) {
    //        if (e.buttonID === "EvaluationEditBtn")
    //            editEvaluation(s.GetRowKey(e.visibleIndex), s);
    //        if (e.buttonID === "EvaluationDeleteBtn") {
    //            if (checkReadOnlyMode())
    //                return;
    //            if (confirm("Remove Evaluation?")) {
    //                var rowIndex = s.GetFocusedRowIndex();
    //                callbackHelper.DoCallback(detailsCallbackPanel, serializeArgs(["DeleteEntry", "Evaluation", rowIndex >= 0 ? s.GetRowKey(rowIndex) : ""]), s);
    //            }
    //        }
    //    }

    //    function taskGrid_CustomButtonClick(s, e) {
    //        if (e.buttonID === "EditBtn")
    //            editTask(s.GetRowKey(e.visibleIndex), s);
    //        if (e.buttonID === "DeleteBtn") {
    //            if (checkReadOnlyMode())
    //                return;
    //            if (confirm("Remove Task?")) {
    //                var rowIndex = s.GetFocusedRowIndex();
    //                callbackHelper.DoCallback(detailsCallbackPanel, serializeArgs(["DeleteEntry", "Task", rowIndex >= 0 ? s.GetRowKey(rowIndex) : ""]), s);
    //            }
    //        }
    //    }

    //    function editEvaluation(id, sender) {
    //        showClearedPopup(evaluationEditPopup);
    //        callbackHelper.DoCallback(evaluationEditPopup, id, sender);
    //    }
    //    function getSelectedEmployeeID() {
    //        var getIndex, getKey;
    //        if (isGridViewMode()) {
    //            getIndex = employeesGrid.GetFocusedRowIndex.aspxBind(employeesGrid);
    //            getKey = employeesGrid.GetRowKey.aspxBind(employeesGrid);
    //        } else {
    //            getIndex = employeeCardView.GetFocusedCardIndex.aspxBind(employeeCardView);
    //            getKey = employeeCardView.GetCardKey.aspxBind(employeeCardView);
    //        }
    //        if (getIndex() >= 0)
    //            return getKey(getIndex());
    //        return null;
    //    };
    //    function getViewMode() {
    //        return getViewModeCore("EmployeeViewMode");
    //    };
    //    function setViewMode(value) {
    //        setViewModeCore("EmployeeViewMode", value);
    //    };
    //    function isGridViewMode() {
    //        var viewMode = getViewMode();
    //        return !viewMode || viewMode === "GridView";
    //    };
    //    function getSelectedItemID() {
    //        return getSelectedEmployeeID();
    //    }

    //    return {
    //        ToolbarMenu_ItemClick: toolbarMenu_ItemClick,
    //        EmployeesGrid_Init: employeesGrid_Init,
    //        EmployeesGrid_FocusedRowChanged: employeesGrid_FocusedRowChanged,
    //        EmployeesGrid_EndCallback: employeesGrid_EndCallback,
    //        EmployeesGrid_ContextMenuItemClick: employeesGrid_ContextMenuItemClick,
    //        GridEditButton_Click: gridEditButton_Click,
    //        EmployeeEditButton_Click: employeeEditButton_Click,
    //        EvaluationGrid_CustomButtonClick: evaluationGrid_CustomButtonClick,
    //        TaskGrid_CustomButtonClick: taskGrid_CustomButtonClick,
    //        GetSelectedItemID: getSelectedItemID,
    //        IsGridViewMode: isGridViewMode
    //    };
    //})();

    //var customerPage = (function () {
    //    function toolbarMenu_ItemClick(s, e) {
    //        switch (e.item.name) {
    //            case "ColumnsCustomization":
    //                if (customerGrid.IsCustomizationWindowVisible())
    //                    customerGrid.HideCustomizationWindow();
    //                else
    //                    customerGrid.ShowCustomizationWindow(e.htmlElement);
    //                break;
    //            case "New":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "insert new customer");
    //                break;
    //            case "Delete":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "delete customer");
    //                break;
    //            case "ShowPivotGrid":
    //                showPivotGrid();
    //                break;
    //        }
    //    }

    //    function gridEditButton_Click(e) {
    //        showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "edit customer's");
    //    };

    //    function customerGrid_FocusedRowChanged(s, e) {
    //        updateDetailInfo(s);
    //    }

    //    function customerGrid_ContextMenuItemClick(s, e) {
    //        switch (e.item.name) {
    //            case "NewRow":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "insert new customer");
    //                e.handled = true;
    //                break;
    //            case "EditRow":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "edit customer's");
    //                e.handled = true;
    //                break;
    //            case "DeleteRow":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "delete customer");
    //                e.handled = true;
    //                break;
    //        }
    //    }

    //    function customerEmployeeButton_Click(s, e) {
    //        startEditCustomerEmployee(s.cpCustomerEmployeeID, s);
    //    }
    //    function startEditCustomerEmployee(id, sender) {
    //        showClearedPopup(customerEmployeeEditPopup);
    //        callbackHelper.DoCallback(customerEmployeeEditPopup, id, sender);
    //    }
    //    function sliderMenu_ItemClick(s, e) {
    //        if (e.item.name === "Root")
    //            return;
    //        ASPxClientUtils.SetCookie("CustomerImageSliderMode", e.item.name);
    //        updateDetailInfo(s);
    //    }

    //    function getSelectedItemID() {
    //        var rowIndex = customerGrid.GetFocusedRowIndex();
    //        return rowIndex >= 0 ? customerGrid.GetRowKey(rowIndex) : null;
    //    }

    //    return {
    //        ToolbarMenu_ItemClick: toolbarMenu_ItemClick,
    //        GridEditButton_Click: gridEditButton_Click,
    //        CustomerGrid_FocusedRowChanged: customerGrid_FocusedRowChanged,
    //        CustomerGrid_ContextMenuItemClick: customerGrid_ContextMenuItemClick,
    //        CustomerEmployeeButton_Click: customerEmployeeButton_Click,
    //        SliderMenu_ItemClick: sliderMenu_ItemClick,
    //        GetSelectedItemID: getSelectedItemID
    //    };
    //})();

    //var productPage = (function () {
    //    function toolbarMenu_ItemClick(s, e) {
    //        var name = e.item.name;
    //        switch (name) {
    //            case "ColumnsCustomization":
    //                if (productGrid.IsCustomizationWindowVisible())
    //                    productGrid.HideCustomizationWindow();
    //                else
    //                    productGrid.ShowCustomizationWindow(e.htmlElement);
    //                break;
    //            case "New":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "insert new product");
    //                break;
    //            case "Delete":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "delete product");
    //                break;
    //            case "ShowPivotGrid":
    //                showPivotGrid();
    //                break;
    //        }
    //    }
    //    function productGrid_FocusedRowChanged(s, e) {
    //        updateDetailInfo(s);
    //    }
    //    function productGrid_ContextMenuItemClick(s, e) {
    //        switch (e.item.name) {
    //            case "NewRow":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "insert new product");
    //                e.handled = true;
    //                break;
    //            case "EditRow":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "edit product");
    //                e.handled = true;
    //                break;
    //            case "DeleteRow":
    //                showEditMessagePopup(editMessagePopup.cpEditMessageTemplate, "delete product");
    //                e.handled = true;
    //                break;
    //        }
    //    }
    //    function productImageSlider_ThumbnailItemClick(s, e) {
    //        callbackHelper.DoCallback(productPopup, s.GetActiveItemIndex(), s);
    //        productPopup.Show();
    //    }
    //    function productImageUpload_FileUploadStart(s, e) {
    //        e.cancel = checkReadOnlyMode();
    //    }
    //    function productImageUpload_FileUploadComplete(s, e) {
    //        updateDetailInfo(s);
    //    }
    //    function productUploadButton_Click(s, e) {
    //        productImageUpload.Upload();
    //    }
    //    function getSelectedItemID() {
    //        var rowIndex = productGrid.GetFocusedRowIndex();
    //        return rowIndex >= 0 ? productGrid.GetRowKey(rowIndex) : null;
    //    }

    //    return {
    //        ToolbarMenu_ItemClick: toolbarMenu_ItemClick,
    //        ProductGrid_FocusedRowChanged: productGrid_FocusedRowChanged,
    //        ProductGrid_ContextMenuItemClick: productGrid_ContextMenuItemClick,
    //        ProductImageSlider_ThumbnailItemClick: productImageSlider_ThumbnailItemClick,
    //        ProductImageUpload_FileUploadStart: productImageUpload_FileUploadStart,
    //        ProductImageUpload_FileUploadComplete: productImageUpload_FileUploadComplete,
    //        ProductUploadButton_Click: productUploadButton_Click,
    //        GetSelectedItemID: getSelectedItemID
    //    };
    //})();

    //var taskPage = (function () {
    //    function toolbarMenu_ItemClick(s, e) {
    //        var name = e.item.name;
    //        switch (name) {
    //            case "GridView":
    //                if (isGridViewMode())
    //                    return;
    //                setViewMode("GridView");
    //                callbackHelper.DoCallback(mainCallbackPanel, "", s);
    //                break;
    //            case "CardsView":
    //                if (!isGridViewMode())
    //                    return;
    //                setViewMode("CardsView");
    //                callbackHelper.DoCallback(mainCallbackPanel, "", s);
    //                break;
    //            case "New":
    //                taskEditPopup.SetHeaderText("New Task");
    //                addTask("", s);
    //                break;
    //        }
    //    }
    //    function taskGrid_CustomButtonClick(s, e) {
    //        switch (e.buttonID) {
    //            case "EditBtn":
    //                editTask(s.GetRowKey(e.visibleIndex), s);
    //                break;
    //            case "DeleteBtn":
    //                deleteTask(s.GetRowKey(e.visibleIndex), s);
    //                break;
    //        }
    //    }
    //    function tasksGrid_ContextMenuItemClick(s, e) {
    //        switch (e.item.name) {
    //            case "NewRow":
    //                addTask("", s);
    //                e.handled = true;
    //                break;
    //            case "EditRow":
    //                editTask(s.GetRowKey(e.elementIndex), s);
    //                e.handled = true;
    //                break;
    //            case "DeleteRow":
    //                deleteTask(s.GetRowKey(e.elementIndex), s);
    //                e.handled = true;
    //                break;
    //        }
    //    }

    //    function viewButton_Click(s, e) {
    //        performTaskCommand("Show", s.cpTaskID, s);
    //    }
    //    function editButton_Click(s, e) {
    //        editTask(s.cpTaskID, s);
    //    }
    //    function deleteButton_Click(s, e) {
    //        deleteTask(s.cpTaskID, s);
    //    }

    //    function getViewMode() {
    //        return getViewModeCore("TaskViewMode");
    //    }
    //    function setViewMode(value) {
    //        setViewModeCore("TaskViewMode", value);
    //    }
    //    function isGridViewMode() {
    //        var viewMode = getViewMode();
    //        return !viewMode || viewMode === "GridView";
    //    }
    //    return {
    //        ToolbarMenu_ItemClick: toolbarMenu_ItemClick,
    //        TaskGrid_CustomButtonClick: taskGrid_CustomButtonClick,
    //        TasksGrid_ContextMenuItemClick: tasksGrid_ContextMenuItemClick,
    //        ViewButton_Click: viewButton_Click,
    //        EditButton_Click: editButton_Click,
    //        DeleteButton_Click: deleteButton_Click,
    //        IsGridViewMode: isGridViewMode
    //    };
    //})();
    var SystCodePage = (function () {
        //debugger;
        function constructor() {
            DemoState = { View: "MailList" };
        }
        function test(s, e) {
        }
        function Init(s, e) {
            var key = getUrlVars()["ID"];
            //if (!key)
            //    return;
            //ChangeDemoState("MailForm", "EditDraft", key);
            ChangeDemoState("MailList");
            hGeID.Set("GeID", '0');
            //hKeyword.Set("Expr", 'Z');
            UpdateMailGridKeyFolderHash();
        }
        function UpdateMailGridKeyFolderHash() {
        }
        function ShowMenuItems() {
            var view = DemoState.View;
            ClientActionMenu.GetItemByName("New").SetVisible(view != "MailForm");
            ClientActionMenu.GetItemByName("Delete").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("back").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("save").SetVisible(view == "MailForm");
        }
        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            if (state.View == "MailForm")
                ShowForm(state.Command, state.Key);
        }
        function ShowGrid() {
            HideForm();
            testgrid.SetVisible(true);
            ClientLayout_PaneResized();
        }
        function ClientLayout_PaneResized() {
            testgrid.SetHeight(0);
            testgrid.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - TopPanel.GetHeight());
        }
        function HideForm() {
            ClientFormPanel.SetVisible(false);
        }
        
        function toolbarMenu_ItemClick(s, e) {
            debugger;
            var name = e.item.name;
            var state = DemoState;
            switch (name) {
                case "New":
                    var param = DevAV.getparam();
                        if (param == "RawMaterial" || param == "ProductStyle" || param == "MediaType")
                            ChangeDemoState("MailForm", "New", 0);
                        else
                            testgrid.AddNewRow();
                    break;
                case "save":
                    var args = name == "send" ? "SendMail" : "SaveMail";
                    if (state.Command === "EditDraft")
                        args += "|" + state.Key;
                    else
                        args = state.Command + "|" + state.Key;
                    ChangeDemoState("MailList");
                    DoCallback(testgrid, function () {
                        testgrid.PerformCallback(args);
                    });
                    break;
                case "back":
                    ChangeDemoState("MailList");
                    break;
                case "Filter":
                    filterPopup.Show();
                    break;
                case "ExportToXLS":
                    //testgrid.PerformCallback("ExportToXLS");
                    btn.DoClick();
                    break;
            }
        }
        function HideGrid() {
            testgrid.SetVisible(false);
        }
        function ClearForm(value) {
        }
        function ShowForm(command, key) {
            //debugger;
            HideGrid();
            ClearForm('All');
            ClientFormPanel.SetVisible(command == "New" || command == "Reply" || command == "EditDraft");
            ClientLayout_PaneResized();
            hGeID.Set('GeID', key);

            //var result = command == "EditDraft" ? "fileManager" : "UploadControl";
            //formLayout.GetItemByName(result).SetVisible(true);
            //grid.PerformCallback('load|' + key);
            if (command == "New") {
                var result = ["load", "0"].join("|");
                //cmbplant.SetEnabled(true);
                //gv.PerformCallback(result);
                //fm.PerformCallback(result);
            }
            if (command == "Reply" || command == "EditDraft") {
                testgrid.GetValuesOnCustomCallback("MailForm|" + command + "|" + key, function (values) {
                    var setValuesFunc = function () {
                        HideLoadingPanel();
                        if (!values)
                            return;
                        //alert(key);
                        hGeID.Set('GeID', key);
                    };
                    PostponeAction(setValuesFunc, function () { return !!window.CmbProductType });
                });
                ShowLoadingPanel(ClientFormPanel.GetMainElement());
            }
        }
        return {
            Init: Init,
            test: test,
            ShowMenuItems: ShowMenuItems,
            OnStateChanged: OnStateChanged,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var EntryPage = (function () {
        function ShowMenuItems() {
            //debugger;
            var view = DemoState.View;
            ClientActionMenu.GetItemByName("ExportToXLS").SetVisible(true);
            ClientActionMenu.GetItemByName("Filter").SetVisible(true);
            ClientActionMenu.GetItemByName("Approve").SetVisible(true);
        }
        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            //if (state.View == "MailForm")
            //    ShowForm(state.Command, state.Key);
        }
        function ShowGrid() {
        }
        function toolbarMenu_ItemClick(s, e) {
            debugger;
            var name = e.item.name;
            if (name == "Filter")
                filterPopup.Show();
            if (name == "ExportToXLS") {
                //testgrid.PerformCallback("ExportToXLS");
                btn.DoClick();
            }
        }
        return {
            ShowMenuItems: ShowMenuItems,
            OnStateChanged: OnStateChanged,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var paramPage = (function () {
        function ShowMenuItems() {
            //debugger;
            var view = DemoState.View;
            ClientActionMenu.GetItemByName("New").SetVisible(true);
            ClientActionMenu.GetItemByName("Filter").SetVisible(true);
        }
        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            //if (state.View == "MailForm")
            //    ShowForm(state.Command, state.Key);
        }
        function ShowGrid() {
        }
        function toolbarMenu_ItemClick(s, e) {
            var name = e.item.name;
            switch (name) {
                case "Filter":
                    //filterPopup.Show();
                    break;
                case "New":
                    gvResult.AddNewRow();
                    //alert("test");
                    break;
            }
        }
        return {
            ShowMenuItems: ShowMenuItems,
            OnStateChanged: OnStateChanged,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var OEEPage = (function () {
        function constructor() {
            DemoState = { View: "MailList" };
        }
        function test(s, e) {
        }
        function UpdateMailGridKeyFolderHash() {
            var hash = {};
            for (var folderName in testgrid.cpVisibleMailKeysHash) {
                var keys = testgrid.cpVisibleMailKeysHash[folderName];
                if (!keys || keys.length == 0)
                    continue;
                hash[folderName] = [];
                for (var i = 0; i < keys.length; i++)
                    hash[keys[i]] = folderName;
            }
            testgrid.cpKeyFolderHash = hash;
        }
        function Init(s, e) {
            //debugger;
            //var key = getUrlVars()["ID"];
            //if (!key)
            //    return;
            //ChangeDemoState("MailForm", "EditDraft", key);
            ChangeDemoState("MailList");
            hGeID.Set("GeID", '0');
            //hKeyword.Set("Expr", 'Z');
            UpdateMailGridKeyFolderHash();
        }

        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            if (state.View == "MailForm")
                ShowForm(state.Command, state.Key);
        }
        function ShowGrid() {
            HideForm();
            testgrid.SetVisible(true);
            PaneResized();
        }
        function HideForm() {
            ClientFormPanel.SetVisible(false);
        }
        function HideGrid() {
            testgrid.SetVisible(false);
        }
        function ClearForm(value) {
            var today = DevAV.currentdate();
            deAnyDate.SetText(today);
        }
        function PaneResized() {
            testgrid.SetHeight(0);
            //var test = ASPxClientUtils.GetDocumentClientHeight();
            testgrid.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 80);
            ClientFormPanel.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 80);
        }
        function ShowMenuItems() {
            //debugger;
            var view = DemoState.View;
            ClientActionMenu.GetItemByName("New").SetVisible(view != "MailForm");
            ClientActionMenu.GetItemByName("Delete").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("back").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("save").SetVisible(view == "MailForm");
            ClientActionMenu.GetItemByName("Filter").SetVisible(view == "MailList");
            var x = document.getElementById("myfooter");
            //topPanel.SetVisible(view != "MailForm");
            //if (view == "MailForm") {
            //    x.style.display = "none";
            //} else {
            //    x.style.display = "block";
            //}
            ClientActionMenu.GetItemByName("ExportToXLS").SetVisible(view == "MailList");
        }
        function toolbarMenu_ItemClick(s, e) {
            //debugger;
            var name = e.item.name;
            var state = DemoState;
            switch (name) {
                case "Filter":
                    //debugger;
                    //filterPopup.SetContentHtml(null);
                    filterPopup.Show();
                    break;
                case "New":
                    ChangeDemoState("MailForm", "New", 0); break;
                case "back":
                    ChangeDemoState("MailList");
                    break;
                case "save":
                    break;
            }
        }
        function ShowForm(command, key) {
            //debugger;
            HideGrid();
            ClearForm('All');
            ClientFormPanel.SetVisible(command == "New" || command == "Reply" || command == "EditDraft");
            PaneResized();
            if (command == "New") {
                var result = ["load", "0"].join("|");
                //cmbplant.SetEnabled(true);
                //gv.PerformCallback(result);
                //fm.PerformCallback(result);
            }
        }
        return {
            Init: Init,
            test: test,
            OnStateChanged: OnStateChanged,
            ShowMenuItems: ShowMenuItems,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var ResultPage = (function () {
        function constructor() {
            DemoState = { View: "MailList" };
        }
        function test(s, e) {
            if (!s.cpSelectedRowKey)
                return;
            delete s.cpSelectedRowKey;
        }
        function UpdateMailGridKeyFolderHash() {
            var hash = {};
            for (var folderName in testgrid.cpVisibleMailKeysHash) {
                var keys = testgrid.cpVisibleMailKeysHash[folderName];
                if (!keys || keys.length == 0)
                    continue;
                hash[folderName] = [];
                for (var i = 0; i < keys.length; i++)
                    hash[keys[i]] = folderName;
            }
            testgrid.cpKeyFolderHash = hash;
        }
        function Init(s, e) {
            //debugger;
            //var key = getUrlVars()["ID"];
            //if (!key)
            //    return;
            //ChangeDemoState("MailForm", "EditDraft", key);
            ChangeDemoState("MailList");
            hGeID.Set("GeID", '0');
            //hKeyword.Set("Expr", 'Z');
            UpdateMailGridKeyFolderHash();
        }

        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            if (state.View == "MailForm")
                ShowForm(state.Command, state.Key);
        }
        function ShowGrid() {
            HideForm();
            testgrid.SetVisible(true);
            PaneResized();
        }
        function HideForm() {
            ClientFormPanel.SetVisible(false);
        }
        function HideGrid() {
            testgrid.SetVisible(false);
        }
        function ClearForm(value) {
            //txtncpid.SetText("");
            var today = DevAV.currentdate();
            deKeyDate.SetText(today);
            deSampleDate.SetText(today);
            cmbplant.SetText("");
            cmbplant.SetEnabled(true);
            txtCook.SetText("");
            txtTemp.SetText("");
            cbShift.SetSelectedIndex(0);
        }
        function PaneResized() {
            testgrid.SetHeight(0);
            //var test = ASPxClientUtils.GetDocumentClientHeight();
            testgrid.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 80);
            ClientFormPanel.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 80);
        }
        function ShowMenuItems() {
            //debugger;
            var view = DemoState.View;
            ClientActionMenu.GetItemByName("New").SetVisible(view != "MailForm");
            ClientActionMenu.GetItemByName("Delete").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("back").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("save").SetVisible(view == "MailForm");
            ClientActionMenu.GetItemByName("Filter").SetVisible(view == "MailList");
            var x = document.getElementById("myfooter");
            //topPanel.SetVisible(view != "MailForm");
            //if (view == "MailForm") {
            //    x.style.display = "none";
            //} else {
            //    x.style.display = "block";
            //}
            ClientActionMenu.GetItemByName("ExportToXLS").SetVisible(view == "MailList");
        }
        function toolbarMenu_ItemClick(s, e) {
            //debugger;
            var name = e.item.name;
            var state = DemoState;
            switch (name) {
                case "Filter":
                    //debugger;
                    //filterPopup.SetContentHtml(null);
                    filterPopup.Show();
                    break;
                case "New":
                    ChangeDemoState("MailForm", "New", 0); break;
                case "back":
                    ChangeDemoState("MailList");
                    break;
                case "save":
                    var args = name == "send" ? "SendMail" : "SaveMail";
                    if (state.Command === "EditDraft")
                        args += "|" + state.Key;
                    else
                        args = state.Command + "|" + state.Key;
                    ChangeDemoState("MailList");
                    DoCallback(testgrid, function () {
                        //grid.UpdateEdit();
                        testgrid.PerformCallback(args);
                        UploadControl.Upload();
                    });
                    break;
            }
        }
        function ShowForm(command, key) {
            //debugger;
            HideGrid();
            ClearForm('All');
            ClientFormPanel.SetVisible(command == "New" || command == "Reply" || command == "EditDraft");
            PaneResized();
            hGeID.Set('GeID', key);

            var result = command == "EditDraft" ? "fileManager" : "UploadControl";
            formLayout.GetItemByName(result).SetVisible(true);
            //grid.PerformCallback('load|' + key);
            if (command == "New") {
                var result = ["load", "0"].join("|");
                cmbplant.SetEnabled(true);
                //gv.PerformCallback(result);
                //fm.PerformCallback(result);
            }
            if (command == "Reply" || command == "EditDraft") {
                testgrid.GetValuesOnCustomCallback("MailForm|" + command + "|" + key, function (values) {
                    var setValuesFunc = function () {
                        HideLoadingPanel();
                        if (!values)
                            return;
                        //alert(key);
                        hGeID.Set('GeID', key);
                        cmbplant.SetText(values["Plant"]);
                        cmbplant.SetEnabled(false);
                        deSampleDate.SetText(values["SampleDate"])
                        txtCook.SetText(values["CoolTime"]);
                        txtTemp.SetText(values["Temp"]);
                        heditor.Set("editor", values["editor"]);
                        deKeyDate.SetText(values["KeyDate"]);
                        //dePrddate.SetText(values["ProductionDate"]);
                        //Clienttimes.SetText(values["Times"]);
                        //Cmblocation.SetText(values["Location"]);
                        //ClientShift.SetText(values["ShiftOption"]);
                        //ClientLines.SetText(values["LinesNo"]);
                        //CmbRecorder.SetValue(values["Recorder"]);
                        cbShift.SetSelectedIndex(values["Shifts"] == "DS" ? 0 : 1);
                        lvwd.PerformCallback(['load', key].join('|'));
                        //grid.PerformCallback(['load', key].join('|'));
                        //fileManager.SetVisible(true);
                        fileManager.PerformCallback('load|' + key);

                    };
                    PostponeAction(setValuesFunc, function () { return !!window.CmbType });
                });
                ShowLoadingPanel(ClientFormPanel.GetMainElement());
            }
        }
        return {
            Init: Init,
            test: test,
            OnStateChanged: OnStateChanged,
            ShowMenuItems: ShowMenuItems,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var LabPage = (function () {
        function constructor() {
            DemoState = { View: "MailList" };
        }
        function test(s, e) {
            if (!s.cpSelectedRowKey)
                return;
            var key = s.cpSelectedRowKey;
            debugger;
            var args = key.split("|");
            if (args[0] == "SaveMail") {
                gv.PerformCallback(key);
            }
            delete s.cpSelectedRowKey;
            //if (key == 0)
            //    ChangeDemoState("MailList");
            //else
            //    ChangeDemoState("MailForm", "EditDraft", key);
        }
        function UpdateMailGridKeyFolderHash() {
            var hash = {};
            for (var folderName in testgrid.cpVisibleMailKeysHash) {
                var keys = testgrid.cpVisibleMailKeysHash[folderName];
                if (!keys || keys.length == 0)
                    continue;
                hash[folderName] = [];
                for (var i = 0; i < keys.length; i++)
                    hash[keys[i]] = folderName;
            }
            testgrid.cpKeyFolderHash = hash;
        }
        function Init(s, e) {
            //debugger;
            //var key = getUrlVars()["ID"];
            //if (!key)
            //    return;
            //ChangeDemoState("MailForm", "EditDraft", key);
            ChangeDemoState("MailList");
            hGeID.Set("GeID", '0');
            //hKeyword.Set("Expr", 'Z');
            UpdateMailGridKeyFolderHash();
        }

        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            if (state.View == "MailForm")
                ShowForm(state.Command, state.Key);
        }
        function ShowGrid() {
            HideForm();
            testgrid.SetVisible(true);
            PaneResized();
        }
        function HideForm() {
            ClientFormPanel.SetVisible(false);
        }
        function HideGrid() {
            testgrid.SetVisible(false);
        }
        function ClearForm(value) {
            txtncpid.SetText("");
            cmbplant.SetText("");
            cmbplant.SetEnabled(true);
            ClientTypes.SetText("");
            ClientTypes.SetEnabled(true);
            var today = DevAV.currentdate();
            deKeyDate.SetText(today);
            dePrddate.SetText(today);
            Clienttimes.SetValue("");
            Cmblocation.SetValue("");
            ClientShift.SetValue("");
            ClientLines.SetValue("");
            CmbRecorder.SetValue("");
            cbShift.SetSelectedIndex(-1);
            CmbMaterialType.SetValue("");
            //cbMaterialType.SetSelectedIndex(-1);
            ClientTypes.SetValue("");
            cmbApprove1.SetValue("");
            cmbApprove2.SetValue("");
            tbHoldQuantity.SetText("");
            tbProblemqty.SetText("");
            cmbFirstDecision.SetValue("");
            mResultDecision.SetText("");
            cmbdecision.SetValue("");
            txtAction.SetText("");
            mRemark.SetText("");

        }
        function PaneResized() {
            testgrid.SetHeight(0);
            //var test = ASPxClientUtils.GetDocumentClientHeight();
            testgrid.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 90);
            ClientFormPanel.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 90);
        }
        function ShowMenuItems() {
            //debugger;
            var hedit = heditor.Get("editor");
            var view = DemoState.View;
            var state = DemoState;
            ClientActionMenu.GetItemByName("New").SetVisible(view != "MailForm");
            ClientActionMenu.GetItemByName("Delete").SetVisible(view != "MailList" && state.Key != 0);
            ClientActionMenu.GetItemByName("back").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("save").SetVisible(view == "MailForm" && hedit!=0);
            ClientActionMenu.GetItemByName("Filter").SetVisible(view == "MailList");
            var x = document.getElementById("myfooter");
            //topPanel.SetVisible(view != "MailForm");
            //if (view == "MailForm") {
            //    x.style.display = "none";
            //} else {
            //    x.style.display = "block";
            //}
            var b = view == "MailList" || (view == "MailForm" && state.Key != 0);
            ClientActionMenu.GetItemByName("ExportToXLS").SetVisible(b);
        }
        function toolbarMenu_ItemClick(s, e) {
            //debugger;
            var name = e.item.name;
            var state = DemoState;
            switch (name) {
                case "Filter":
                    //debugger;
                    //filterPopup.SetContentHtml(null);
                    filterPopup.Show();
                    break;
                case "New":
                    ChangeDemoState("MailForm", "New", 0); break;
                case "ExportToXLS":
                    //testgrid.PerformCallback("ExportToXLS|" + state.Key);
                    btn.DoClick();
                    break;
                case "back":
                    ChangeDemoState("MailList");
                    break;
                case "save":
                    if (window.testgrid && !ASPxClientEdit.ValidateEditorsInContainerById("MailForm"))
                        return;
                    if (!ASPxClientEdit.ValidateGroup('group1'))
                        return alert('Field is required');
                    var args = name == "send" ? "SendMail" : "SaveMail";
                    if (state.Command === "EditDraft")
                        args += "|" + state.Key;
                    else
                        args = state.Command + "|" + state.Key;
                    ChangeDemoState("MailList");
                    DoCallback(testgrid, function () {
                        //grid.UpdateEdit();
                        window.setTimeout(function () {
                            gv.UpdateEdit();
                        }, 0);
                        testgrid.PerformCallback(args);
                        UploadControl.Upload(); 
                    });
                    break;
                case "Delete":
                    if (!window.confirm("Confirm Delete?"))
                        return;
                    var rowIndex = testgrid.GetFocusedRowIndex();
                    var keys = rowIndex >= 0 ? testgrid.GetRowKey(rowIndex) : null;
                    if (state.View == "MailForm") {
                        keys = [state.Key];
                        ChangeDemoState("MailList");
                    }
                    testgrid.PerformCallback("Delete|" + keys);
                    break;
            }
        }
        function ShowForm(command, key) {
            //debugger;
            HideGrid();
            ClearForm('All');
            ClientFormPanel.SetVisible(command == "New" || command == "Reply" || command == "EditDraft");
            PaneResized();
            hGeID.Set('GeID', key);
            var result = command == "EditDraft" ? "fileManager" : "UploadControl";
            formLayout.GetItemByName(result).SetVisible(true);

            grid.PerformCallback('load|' + key);
            if (command == "New") {
                var result = ["load", "0"].join("|");
                hGeID.Set('GeID', '0');
                cmbplant.SetEnabled(true);
                gv.PerformCallback(result);
                //fm.PerformCallback(result);
            }
            if (command == "Reply" || command == "EditDraft") {
                testgrid.GetValuesOnCustomCallback("MailForm|" + command + "|" + key, function (values) {
                    var setValuesFunc = function () {
                        HideLoadingPanel();
                        if (!values)
                            return;
                        //alert(key);
                        hGeID.Set('GeID', key);
                        var r = values["PlantID"].toString();
                        cmbplant.SetValue(r);
                        OnPlantChanged(r);
                        cmbplant.SetEnabled(false);
                        //OnGetApprove(r);
                        heditor.Set("editor", values["editor"]);
                        txtncpid.SetText(values["NCPID"]);
                        ClientTypes.SetText(values["TypeNCP"]);
                        //Cmblocation.PerformCallback(['Location', values["TypeNCP"], r.toString()].join('|'));
                        ClientTypes.SetEnabled(false);
                        deKeyDate.SetText(values["KeyDate"]);
                        dePrddate.SetText(values["ProductionDate"]);
                        Clienttimes.SetText(values["Times"]);
                        Cmblocation.SetText(values["Location"]);
                        ClientShift.SetText(values["ShiftOption"]);
                        ClientLines.SetText(values["LinesNo"]);
                        CmbRecorder.SetValue(values["Recorder"]);
                        cbShift.SetSelectedIndex(values["Shift"] == "DS" ? 0 : 1);
                        debugger;
                        //cbMaterialType.SetValue(values["matType"]);
                        //hMaterialType.Set("MaterialType", values["matType"]);
                        CmbMaterialType.PerformCallback('c|' + values["matType"]);
                        //gv.PerformCallback('c|' + values["matType"]);
                        //cmbApprove1.PerformCallback('reload|' + values["Approve"]);
                        //cmbApprove2.PerformCallback('reload|' + values["Approvefinal"]);
                        mRemark.SetText(values["Remark"]);
                        mResultDecision.SetText(values["ResultDecision"]);
                        txtAction.SetText(values["Action"]);

                        cmbFirstDecision.SetValue(values["FirstDecision"]);
                        cmbdecision.SetValue(values["Decision"]);
                        tbProblemqty.SetText(values["ProblemQty"]);
                        tbHoldQuantity.SetText(values["Quantity"]);
                        if (values["HoldQuantity"]!= '')
                        cbOptfull.SetSelectedIndex(values["HoldQuantity"]);
                        //gv.PerformCallback(['load', key].join('|'));
                        //grid.PerformCallback(['load', key].join('|'));
                        //var b = true;
                        //fileManager.PerformCallback(['load', b].join("|"));

                    };
                    PostponeAction(setValuesFunc, function () { return !!window.CmbMaterialType });
                });
                ShowLoadingPanel(ClientFormPanel.GetMainElement());
            }
        }
        return {
            Init: Init,
            test: test,
            OnStateChanged: OnStateChanged,
            ShowMenuItems: ShowMenuItems,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var chatsPage = (function () {
        function constructor() {
            DemoState = { View: "MailList" };
        }
        function OnStateChanged() {
            var state = DemoState;
            //if (state.View == "MailList")
            //    ShowGrid();
            //if (state.View == "MailForm")
            //    ShowForm(state.Command, state.Key);
        }
        function ShowMenuItems() {
            //debugger;
            var view = DemoState.View;
            //ClientActionMenu.GetItemByName("New").SetVisible(view != "MailForm");
            //ClientActionMenu.GetItemByName("Delete").SetVisible(view != "MailList");
            //ClientActionMenu.GetItemByName("back").SetVisible(view != "MailList");
            //ClientActionMenu.GetItemByName("save").SetVisible(view == "MailForm");
            ClientActionMenu.GetItemByName("Filter").SetVisible(view == "MailList");
            var x = document.getElementById("myfooter");
            //topPanel.SetVisible(view != "MailForm");
            //if (view == "MailForm") {
            //    x.style.display = "none";
            //} else {
            //    x.style.display = "block";
            //}
            ClientActionMenu.GetItemByName("ExportToXLS").SetVisible(view == "MailList");
        }
        return {
            //Init: Init,
            //test: test,
            OnStateChanged: OnStateChanged,
            ShowMenuItems: ShowMenuItems
            //ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var InprocessPage = (function () {
        //debugger;
        function constructor() {
            DemoState = { View: "MailList" };
        }
        function Init(s, e) {
            //var key = getUrlVars()["ID"];
            //if (!key)
            //    return;
            //ChangeDemoState("MailForm", "EditDraft", key);
            ChangeDemoState("MailList");
            hGeID.Set("GeID", '0');
            //hKeyword.Set("Expr", 'Z');
            UpdateMailGridKeyFolderHash();
        }
        function test(s, e) {
            //debugger;
            var txx = hKeyword.Get("Expr");
            //alert(txx);
            var key = testgrid.cpKeyValue;
            if (key != undefined || key != null) {
                var args = key.split("|");
                if (args[0] == 'filter') {
                    hKeyword.Set("Expr", args[1] == '' ? 'Z' : args[1]);
                    s.PerformCallback('rebind');
                }
            }
            testgrid.cpKeyValue = null;
            if (key == 0)
                ChangeDemoState("MailList");
        }
        function UpdateMailGridKeyFolderHash() {
            var hash = {};
            for (var folderName in testgrid.cpVisibleMailKeysHash) {
                var keys = testgrid.cpVisibleMailKeysHash[folderName];
                if (!keys || keys.length == 0)
                    continue;
                hash[folderName] = [];
                for (var i = 0; i < keys.length; i++)
                    hash[keys[i]] = folderName;
            }
            testgrid.cpKeyFolderHash = hash;
        }
        function toolbarMenu_ItemClick(s, e) {
            //debugger;
            var name = e.item.name;
            var state = DemoState;
            switch (name) {
                case "Filter":
                    //debugger;
                    //filterPopup.SetContentHtml(null);
                    filterPopup.Show();
                    break;
                case "New":
                    ChangeDemoState("MailForm", "New", 0); break;
                case "back":
                    ChangeDemoState("MailList");
                    break;
                case "Delete":
                    //debugger;
                    if (!window.confirm("Confirm Delete?"))
                        return;
                    var rowIndex = testgrid.GetFocusedRowIndex();
                    var keys = rowIndex >= 0 ? testgrid.GetRowKey(rowIndex) : null;
                    if (state.View == "MailForm") {
                        keys = [state.Key];
                        ChangeDemoState("MailList");
                    }
                    testgrid.PerformCallback("Delete|" + keys);
                    //alert("Delete|" + keys);
                    break;
                case "save":
                    var args = name == "send" ? "SendMail" : "SaveMail";
                    if (state.Command === "EditDraft")
                        args += "|" + state.Key;
                    if (state.Command === "New")
                        args += "|" + 0;
                    ChangeDemoState("MailList");
                    DoCallback(testgrid, function () {
                        testgrid.PerformCallback(args);
                        gv.UpdateEdit();
                        //uploader.Upload(); 
                    });
                    break;
            }
        }
        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            if (state.View == "MailForm")
                ShowForm(state.Command, state.Key);
        }
        function ShowGrid() {
            HideForm();
            testgrid.SetVisible(true);
            PaneResized();
        }
        function HideForm() {
            ClientFormPanel.SetVisible(false);
        }
        function ShowForm(command, key) {
            //debugger;
            HideGrid();
            ClearForm('All');
            ClientFormPanel.SetVisible(command == "New" || command == "Reply" || command == "EditDraft");
            PaneResized();
            //if (command == "New") {
            //    testgrid.GetValuesOnCustomCallback("MailForm|" + command + "|" + key, function (values) {
            //        var setValuesFunc = function () {
            //            HideLoadingPanel();
            //            if (!values)
            //                return;
            //            hGeID.Set('GeID', 0);
            //            ClientSampleID.SetText('#######');
            //            testgrid.PerformCallback('AddRow||0');
            //         };
            //        PostponeAction(setValuesFunc, function () { return !!window.CmbMaterial });
            //    });
            //    ShowLoadingPanel(ClientFormPanel.GetMainElement());
            //}
            hGeID.Set('GeID', key);
            grid.PerformCallback('load|' + key);
            gvTemp.PerformCallback('load|' + key);
            if (command == "New") {
                var result = ["load", "0"].join("|");
                gv3.PerformCallback(result);
                gv.PerformCallback(result);
            }
            if (command == "Reply" || command == "EditDraft") {
                testgrid.GetValuesOnCustomCallback("MailForm|" + command + "|" + key, function (values) {
                    var setValuesFunc = function () {
                        HideLoadingPanel();
                        if (!values)
                            return;
                        //alert(key);
                        //debugger;
                        txtGetID.SetText(key);
                        hGeID.Set('GeID', key);//
                        ClientSampleID.SetText(values["SampleID"]);
                        ClientRequestNo.SetText(values["SampleID"]);
                        CmbMaterial.SetText(values["Material"]);
                        deAnyDate.SetText(values["ReceivingDate"]);

                        cmbSupplier.SetValue(values["Supplier"]);
                        ClientSpecies.SetText(values["Species"]);
                        ClientStyle.SetText(values["Style"]);
                        //if (values["Material"] != '') {
                        //    ClientSize.SetValue(values["Material"].substring(5, 6));
                        //}
                        CmbMaterial.SetText(values["Material"]);
                        cbShift.SetSelectedIndex(values["Shifts"] == "DS" ? 0 : 1);

                        txtBatch.SetText(values["Batch"]);
                        txtVessel.SetText(values["Vessel"]);
                        txtInvoice.SetText(values["InvoiceNo"]);
                        txtContainer.SetText(values["ContainerNo"]);
                        txtNetWeight.SetText(values["NetWeight"]);
                        cmbPackaging.SetValue(values["Packaging"]);
                        mNotes.SetText(values["Notes"]);

                        //rbOdor.SetSelectedIndex(values["Odor"] == "NORMAL" ? 0 : 1);
                        //tbOdor.SetVisible(rbOdor.GetSelectedItem().text == 'AB NORMAL');
                        //rbTexture.SetSelectedIndex(values["Texture"] == "NORMAL" ? 0 : 1);
                        //tbTexture.SetVisible(rbTexture.GetSelectedItem().text == 'AB NORMAL');
                        //CmbFormalin.SetValue(values["Formalin"]);

                        var a = values["traCondition"].split(";"), i;
                        for (i = 0; i < a.length; i++) {
                            if (a[i] != "") {
                                //var selValues = a[i];
                                var indexes = [i];
                                cbtraCondition.SelectIndices(indexes);
                                //cbtraCondition.SelectValues(selValues);
                            }
                        }
                        //cbtraCondition.SetValue(values["traCondition"]);
                        //txtCount.SetText(values["Countper"]);
                        //txtCount.SetEnabled(values["Countper"] != "");

                        //mOthers.SetText(values["Others"]);
                        tbThermometer.SetText(values["Thermometer"]);
                        rbIceId.SetValue(values["IceId"]);
                        tbSampling.SetText(values["Sampling"]);
                        gvApp.PerformCallback('reload|' + key+"|6");
                        gv3.PerformCallback('load|' + key);
                        //if (values["valWeight"] != '0')
                        radioButtonList.SetSelectedIndex(0);
                        gv.PerformCallback('load|' + key);
                        };
                    PostponeAction(setValuesFunc, function () { return !!window.CmbMaterial });
                });
                ShowLoadingPanel(ClientFormPanel.GetMainElement());
            }
        }
        function HideGrid() {
            testgrid.SetVisible(false);
        }
        function ClearForm(value) {
            txtGetID.SetText(0);
            var today = DevAV.currentdate();
            deAnyDate.SetText(today);
            ClientRequestNo.SetText('#######');
            CmbMaterial.SetText("");
            cmbSupplier.SetValue("");
            ClientSpecies.SetValue("");
            ClientStyle.SetValue("");
            cbShift.SetSelectedIndex(1);

            txtBatch.SetText("");
            txtVessel.SetText("");
            txtInvoice.SetText("");
            txtContainer.SetText("");
            txtNetWeight.SetText("");
            cmbPackaging.SetValue("");
            mNotes.SetText("");

            //rbOdor.SetSelectedIndex(1);
            //tbOdor.SetVisible(rbOdor.GetSelectedItem().text == 'AB NORMAL');
            //rbTexture.SetSelectedIndex(1);
            //tbTexture.SetVisible(rbTexture.GetSelectedItem().text == 'AB NORMAL');
            //CmbFormalin.SetValue("");

            //txtCount.SetText("");
            //txtCount.SetEnabled(txtCount.GetText() != "");

            //mOthers.SetText("");
            tbThermometer.SetText("");
            rbIceId.SetValue("");
            tbSampling.SetText("");
        }
        function PaneResized() {
            testgrid.SetHeight(0);
            //var test = ASPxClientUtils.GetDocumentClientHeight();
            testgrid.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 80);
            ClientFormPanel.SetHeight(ASPxClientUtils.GetDocumentClientHeight() - 80);
        }
        function ShowMenuItems() {
            //debugger;
            var view = DemoState.View;
            ClientActionMenu.GetItemByName("New").SetVisible(view != "MailForm");
            ClientActionMenu.GetItemByName("Delete").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("back").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("save").SetVisible(view == "MailForm");
            ClientActionMenu.GetItemByName("Filter").SetVisible(view == "MailList");
            var x = document.getElementById("myfooter");
            //topPanel.SetVisible(view != "MailForm");
            //if (view == "MailForm") {
            //    x.style.display = "none";
            //} else {
            //    x.style.display = "block";
            //}
            ClientActionMenu.GetItemByName("ExportToXLS").SetVisible(view == "MailList");
        }
        return {
            Init: Init,
            test: test,
            OnStateChanged: OnStateChanged,
            ShowMenuItems: ShowMenuItems,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var countEditPage = (function () {
        //debugger;
        function constructor() {
            DemoState = { View: "MailList" };
        }
        function Init(s, e) {
            //var key = getUrlVars()["ID"];
            //if (!key)
            //    return;
            //ChangeDemoState("MailForm", "EditDraft", key);
            ASPxClientUtils.SetCookie('theme', 'iOS');
            ChangeDemoState("MailList");
            hGeID.Set("GeID", '0');
            //hKeyword.Set("Expr", 'Z');
            UpdateMailGridKeyFolderHash();
        }
        function UpdateMailGridKeyFolderHash() {
            var hash = {};
            for (var folderName in testgrid.cpVisibleMailKeysHash) {
                var keys = testgrid.cpVisibleMailKeysHash[folderName];
                if (!keys || keys.length == 0)
                    continue;
                hash[folderName] = [];
                for (var i = 0; i < keys.length; i++)
                    hash[keys[i]] = folderName;
            }
            testgrid.cpKeyFolderHash = hash;
        }
        function test(s, e) {
            //debugger;
            var txx = hKeyword.Get("Expr");
            //alert(txx);
            var key = testgrid.cpKeyValue;
            if (key != undefined || key != null) {
                var args = key.split("|");
                if (args[0] == 'filter') {
                    hKeyword.Set("Expr", args[1]==''?'Z':args[1]);
                    s.PerformCallback('rebind');
                }
            }
            testgrid.cpKeyValue = null;
            if (key == 0)
                ChangeDemoState("MailList");
        }
        function PaneResized() {
            //debugger;
            testgrid.SetHeight(0);
            var test = ASPxClientUtils.GetDocumentClientHeight();
            testgrid.SetHeight(ASPxClientUtils.GetDocumentClientHeight()-80);
            ClientFormPanel.SetHeight(ASPxClientUtils.GetDocumentClientHeight()-80);
        }
        function toolbarMenu_ItemClick(s, e) {
            var name = e.item.name;
            var state = DemoState;
            debugger;
            switch (name) {
                case "Filter":
                    //debugger;
                    //filterPopup.SetContentHtml(null);
                    filterPopup.Show();
                    break;
                case "scan":
                    OnBtnShowPopupClick('scan');
                    break;
                case "back":
                    grid.CancelEdit();
                    //grid.Refresh();
                    ChangeDemoState("MailList");
                    break;
                case "New":
                    ChangeDemoState("MailForm", "New", 0);
                    //grid.SetVisible(false);
                    //cb.PerformCallback('rebind|0');
                    //showClearedPopup(countEditPopup);
                    hGeID.Set("GeID", '0');
                    grid.PerformCallback('clear');
                    grid.CancelEdit();
                    CmbGrouping.SetEnabled(true);
                    CmbPlant.SetSelectedIndex(0);
                    CmbPlant.SetEnabled(true);
                    deValidfrom.SetDate(new Date());
                    break;
                case "Delete":
                    //debugger;
					if (!window.confirm("Confirm Delete?"))
                    return;
					var rowIndex = testgrid.GetFocusedRowIndex();
                    var keys = rowIndex >= 0 ? testgrid.GetRowKey(rowIndex) : null;
                    if (state.View == "MailForm") {
                        keys = [state.Key];
                        ChangeDemoState("MailList");
                    }
                    grid.CancelEdit();
                    grid.DeleteRow(keys);
                    testgrid.PerformCallback("Delete|" + keys);
 
                    //alert("Delete|" + keys);
                    break;
                case "save":
                    var args = name == "send" ? "SendMail" : "SaveMail";
                    if (state.Command === "EditDraft")
                        args += "|" + state.Key;
                    else
                        args = state.Command + "|" + state.Key;
                    ChangeDemoState("MailList");
                    DoCallback(testgrid, function () {
                        grid.UpdateEdit();
                        testgrid.PerformCallback(args);
                        //uploader.Upload(); 
                    });
                    break;
                case "Approve": case "Reject":
                    testgrid.PerformCallback(name);
                    break;
            }
        }
        function OnStateChanged() {
            var state = DemoState;
            if (state.View == "MailList")
                ShowGrid();
            if (state.View == "MailForm")
                ShowForm(state.Command, state.Key);
        }
        function ShowGrid(){
            //HideLoadingPanel();
            HideForm();
            testgrid.SetVisible(true);
            PaneResized();
        }
        function HideForm() {
            ClientFormPanel.SetVisible(false);
        }
        function ShowForm(command, key) {
            HideGrid();
            ClearForm('All');
            ClientFormPanel.SetVisible(command == "New" || command == "Reply" || command == "EditDraft");
            PaneResized();
            if (command == "Reply" || command == "EditDraft") {
                testgrid.GetValuesOnCustomCallback("MailForm|" + command + "|" + key, function (values) {
                    var setValuesFunc = function () {
                        HideLoadingPanel();
                        if (!values)
                            return;
                        //alert(key);
                        hGeID.Set('GeID', key);//
                        CmbPlant.SetValue(values["Plant"]);
                        CmbPlant.SetEnabled(key == 0);
                        CmbGrouping.SetValue(values["Grouping"]);
                        CmbGrouping.SetEnabled(key == 0);
                        var text = ["Build", values["Grouping"], values["Plant"], values["InstallArea"]];
                        var param = text.join("|");
                        ClientArea.PerformCallback(param);
                        ClientArea.SetValue(values["InstallArea"]);
                        var Group = values["Grouping"];
                        grid.PerformCallback('reload|'+[values["Plant"], Group].join("|"));
                        //grid.StartEditRow(0);
                    };
                    PostponeAction(setValuesFunc, function () { return !!window.CmbPlant });
                });
                ShowLoadingPanel(ClientFormPanel.GetMainElement());
            }
        }
        function HideGrid() {
            testgrid.SetVisible(false);
        }
        function ClearForm(value) {
            //debugger;
            if (value == 'All') {
                CmbPlant.SetValue("");
            }
            CmbGrouping.SetValue("");
            ClientArea.SetValue("");
            //grid.CancelEdit();
            grid.PerformCallback('clear|0');
            //grid.Refresh();
        }
        function ShowMenuItems() {
            //debugger;
            var view = DemoState.View;
            ClientActionMenu.GetItemByName("New").SetVisible(view != "MailForm");
            ClientActionMenu.GetItemByName("Delete").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("back").SetVisible(view != "MailList");
            ClientActionMenu.GetItemByName("save").SetVisible(view == "MailForm");
            ClientActionMenu.GetItemByName("scan").SetVisible(view == "MailForm");
            //ClientActionMenu.GetItemByName("Reject").SetVisible(view == "MailList");
            //ClientActionMenu.GetItemByName("Approve").SetVisible(view == "MailList");
            //ClientActionMenu.GetItemByName("ViewMore").SetVisible(view == "MailForm");
            ClientActionMenu.GetItemByName("Filter").SetVisible(view == "MailList");
            var x = document.getElementById("myfooter");
            //topPanel.SetVisible(view != "MailForm");
            ////debugger;
            //if (view == "MailForm") {
            //    x.style.display = "none";
            //} else {
            //    x.style.display = "block";
            //}
            //ClientActionMenu.GetItemByName("ExportToXLS").SetVisible(view == "MailList");
        }
        return {
            Init: Init,
            test: test,
            ClearForm: ClearForm,
//            OnGridViewSelectionChanged: OnGridViewSelectionChanged,
            OnStateChanged: OnStateChanged,
            ShowMenuItems: ShowMenuItems,
            ToolbarMenu_ItemClick: toolbarMenu_ItemClick
        };
    })();
    var transferPage = (function () {
        function toolbarMenu_ItemClick(s, e) {
            var name = e.item.name;
            //alert("test :" + name);
            switch (name) {
                case "New":
                    hfid.Set('hidden_value', 0);
                    txtBooksNo.SetFocus();
                    //radioButtonList.GetItem(0).value = true;
                    testgrid.PerformCallback('reload|0');
                    transferEditPopup.SetHeaderText("New Task");
                    addTask("", s);
                    break;
            }
        }
        function addTask(employeeID, sender) {
            employeeID = employeeID ? employeeID.toString() : "";
            performTaskCommand("New", employeeID, sender);
        }
        function editTask(id, sender) {
            performTaskCommand("Edit", id, sender);
        }
        function performTaskCommand(commandName, args, sender) {
            showClearedPopup(transferEditPopup);
            //callbackHelper.DoCallback(transferEditPopup, commandName + "|" + args, sender);
        }
        function transferEditPopup_EndCallback(s, e) {
            //alert("testzz");
        }
            return {
                ToolbarMenu_ItemClick: toolbarMenu_ItemClick
            };
        })();        
    function getCurrentPage() {
        //debugger;
        var pageName = DevAVPageName;
        switch (pageName) {
            //case "Dashboard":
            //    return dashboardPage;
            //case "Employees":
            //    return employeePage;
            case "SystCodeForm":
                return SystCodePage;
            case "EntryEdit": case "ReportForm": case "ApproveForm":
                return EntryPage;
            case "param": case "group": case "area": case "InstallArea": case "Supplier": case "Packaging":
            case "user": case "Plant": case "Problem": case "location": case "NCPType":
                return paramPage;
            case "countEditForm":
                return countEditPage;
            case "InprocessForm":
                return InprocessPage;
            case "transfercontrol":
                return transferPage;
            case "OEEForm":
                return OEEPage;
            case "chartsForm":
                return chatsPage;
            case "LabForm":
                return LabPage;
            case "ResultForm":
                return ResultPage;
        }
    };
    var page = getCurrentPage();

    //function gridEditButton_Click(event) {
    //    page.GridEditButton_Click(event);
    //    ASPxClientUtils.PreventEventAndBubble(event);
    //}

    //function adjustMainContentPaneSize() {
    //    var pane = splitter.GetPaneByName("MainContentPane");

    //    if (page === customerPage)
    //        adjustControlSize(pane, customerGrid, detailsCallbackPanel);

    //    if (page === employeePage)
    //        adjustControlSize(pane, page.IsGridViewMode() ? employeesGrid : employeeCardView, detailsCallbackPanel);

    //    if (page === taskPage)
    //        adjustControlSize(pane, page.IsGridViewMode() ? taskGrid : taskCardView);

    //    if (page === productPage)
    //        adjustControlSize(pane, productGrid, detailsCallbackPanel);
    //}
    //function adjustControlSize(splitterPane, grid, detailPanel, minHeight) {
    //    grid.SetHeight(splitterPane.GetClientHeight() - (detailPanel ? detailPanel.GetHeight() : 0));
    //}

    //function filterNavBar_Init(s, e) {
    //    loadFilterNavBarSelectedItem();
    //};
    //function filterNavBar_ItemClick(s, e) {
    //    if (e.item.name !== s.cpPrevSelectedItemName)
    //        changeFilter(s.cpFilterExpressions[e.item.name], s);
    //};

    //function searchBox_KeyDown(s, e) {
    //    window.clearTimeout(searchBoxTimer);
    //    searchBoxTimer = window.setTimeout(function () { onSearchTextChanged(s); }, 1200);
    //    e = e.htmlEvent;
    //    if (e.keyCode === 13) {
    //        if (e.preventDefault)
    //            e.preventDefault();
    //        else
    //            e.returnValue = false;
    //    }
    //};
    //function searchBox_TextChanged(s, e) {
    //    onSearchTextChanged(s);
    //};
    //function onSearchTextChanged(sender) {
    //    window.clearTimeout(searchBoxTimer);
    //    var searchText = searchBox.GetText();
    //    if (hiddenField.Get("SearchText") == searchText)
    //        return;
    //    hiddenField.Set("SearchText", searchText);
    //    callbackHelper.DoCallback(mainCallbackPanel, serializeArgs(["Search"]), sender);
    //};

    function filterControl_Applied(s, e) {
    //    changeFilter(e.filterExpression, s);
    }
    function saveCustomFilterCheckBox_CheckedChanged(s, e) {
    //    customFilterTextBox.SetEnabled(s.GetChecked());
    //    customFilterTextBox.SetIsValid(true);
    }
    function customFilterTextBox_Validation(s, e) {
    //    e.isValid = !!e.value || !saveCustomFilterCheckBox.GetChecked();
    }
    function saveFilterButton_Click(s, e) {
    //    if (saveCustomFilterCheckBox.GetChecked()) {
    //        var validated = ASPxClientEdit.ValidateEditorsInContainer(filterPopup.GetMainElement());
    //        if (validated)
    //            filterPopup.Hide();
    //        return;
    //    }
    //    e.processOnServer = false;
    //    filterPopup.Hide();
    //    filterControl.Apply();
    }
    function cancelFilterButton_Click(s, e) {
        filterPopup.Hide();
    }
    //function mainCallbackPanel_EndCallback(s, e) {
    //    if (s.cpSelectedFilterNavBarItemName)
    //        updateFilterNavBarSelection(s.cpSelectedFilterNavBarItemName);
    //    adjustMainContentPaneSize();
    //}


    //function splitter_PaneResized(s, e) {
    //    if (e.pane.name == 'MainContentPane')
    //        window.setTimeout(function () { adjustMainContentPaneSize(); }, 0);
    //}

    //function pageViewerPopup_Shown(s, e) {
    //    preparePopupWithIframe(s);
    //}
    //function revenueAnalysisPopup_Shown(s, e) {
    //    preparePopupWithIframe(s);
    //}
    //function pageViewerPopup_CloseUp(s, e) {
    //    s.SetContentUrl("");
    //}
    //function revenueAnalysisPopup_CloseUp(s, e) {
    //    s.SetContentUrl("");
    //}
    function Init(s, e) {
        //debugger;
        //ChangeDemoState("MailList");
        page.Init(s, e);
    }
    function toolbarMenu_ItemClick(s, e) {
        var name = e.item.name;
    //    var selectedItemID = page.GetSelectedItemID && page.GetSelectedItemID();
    //    if (name === "Print" || e.item.parent && e.item.parent.name === "Print")
    //        openReport(s.cpReportNames[name], selectedItemID);
    //    if (name === "ExportToSpreadsheet")
    //        openSpreadsheet(s.cpReportNames[name], selectedItemID);
    //    if (name === "Filter")
    //        filterPopup.Show();

        page.ToolbarMenu_ItemClick(s, e);
    }

    //function preparePopupWithIframe(popup) {
    //    var iframe = popup.GetContentIFrame();
    //    setAttribute(iframe, "scrolling", "no");
    //    iframe.style.overflow = "hidden";
    //};

    //function updateFilterNavBarSelection(selectedItemName) {
    //    var oldItem = filterNavBar.GetSelectedItem();
    //    var newItem = filterNavBar.GetItemByName(selectedItemName);
    //    if (oldItem && newItem && filterNavBar.cpFilterExpressions[oldItem.name] === filterNavBar.cpFilterExpressions[newItem.name])
    //        return;
    //    filterNavBar.SetSelectedItem(newItem);
    //    loadFilterNavBarSelectedItem();
    //}

    //function changeFilter(expression, sender) {
    //    callbackHelper.DoCallback(mainCallbackPanel, serializeArgs(["FilterChanged", expression]), sender);
    //    loadFilterNavBarSelectedItem();
    //}

    //function loadFilterNavBarSelectedItem() {
    //    var item = filterNavBar.GetSelectedItem();
    //    filterNavBar.cpPrevSelectedItemName = item ? item.name : "";
    //}

    //function serializeArgs(args) {
    //    var result = [];
    //    for (var i = 0; i < args.length; i++) {
    //        var value = args[i] ? args[i].toString() : "";
    //        result.push(value.length);
    //        result.push("|");
    //        result.push(value);
    //    }
    //    return result.join("");
    //}
    //function setAttribute(element, attrName, value) {
    //    if (element.setAttribute)
    //        element.setAttribute(attrName, value);
    //    else if (element.setProperty)
    //        element.setProperty(attrName, value, "");
    //}

    //function employeeEditPopup_EndCallback(s, e) {
    //    s.SetHeaderText(s.cpHeaderText);
    //    firstNameTextBox.Focus();
    //}
    //function evaluationEditPopup_EndCallback(s, e) {
    //    s.SetHeaderText(s.cpHeaderText);
    //    evaluationSubjectTextBox.Focus();
    //}
    //function taskEditPopup_EndCallback(s, e) {
    //    s.SetHeaderText(s.cpHeaderText);
    //    OwnerComboBox.Focus();
    //}
    //function customerEditPopup_EndCallback(s, e) {
    //    s.SetHeaderText(s.cpHeaderText);
    //    firstNameTextBox.Focus();
    //}

    return {
        Page: page,
        Init: Init,
        OnStateChanged: OnStateChanged,
        ShowMenuItems: ShowMenuItems,
    //    FilterNavBar_Init: filterNavBar_Init,
    //    FilterNavBar_ItemClick: filterNavBar_ItemClick,
    //    SearchBox_KeyDown: searchBox_KeyDown,
    //    SearchBox_TextChanged: searchBox_TextChanged,
        FilterControl_Applied: filterControl_Applied,
        SaveCustomFilterCheckBox_CheckedChanged: saveCustomFilterCheckBox_CheckedChanged,
        CustomFilterTextBox_Validation: customFilterTextBox_Validation,
        SaveFilterButton_Click: saveFilterButton_Click,
        CancelFilterButton_Click: cancelFilterButton_Click,
    //    MainCallbackPanel_EndCallback: mainCallbackPanel_EndCallback,
    //    Splitter_PaneResized: splitter_PaneResized,
    //    PageViewerPopup_Shown: pageViewerPopup_Shown,
    //    PageViewerPopup_CloseUp: pageViewerPopup_CloseUp,
    //    RevenueAnalysisPopup_Shown: revenueAnalysisPopup_Shown,
    //    RevenueAnalysisPopup_CloseUp: revenueAnalysisPopup_CloseUp,
    //    RevenueAnalysisCloseButton_Click: revenueAnalysisCloseButton_Click,
        ToolbarMenu_ItemClick: toolbarMenu_ItemClick,
    //    GridEditButton_Click: gridEditButton_Click,
    //    GridCustomizationWindow_CloseUp: gridCustomizationWindow_CloseUp,
    //    CardView_Init: cardView_Init,
    //    CardView_EndCallback: cardView_EndCallback,
    //    EmployeeCancelButton_Click: employeeCancelButton_Click,
    //    EmployeeSaveButton_Click: employeeSaveButton_Click,
    //    EvaluationSaveButton_Click: evaluationSaveButton_Click,
    //    EvaluationCancelButton_Click: evaluationCancelButton_Click,
    //    TaskSaveButton_Click: taskSaveButton_Click,
    //    TaskCancelButton_Click: taskCancelButton_Click,
        xxxSaveButton_Click: xxxSaveButton_Click,
        xxxCancelButton_Click: xxxCancelButton_Click,
        gridData_RowDblClick: gridData_RowDblClick,
        test: test,
        ChangeDemoState: ChangeDemoState,
        currentdate: currentdate,
        getparam:getparam,
        SetUnitPriceColumnVisibility: SetUnitPriceColumnVisibility,
        showClearedPopup: showClearedPopup,
        FilesContainer: FilesContainer,
        Clear: Clear,
        ClearForm: ClearForm,
        AddFile: AddFile
    //    CustomerCancelButton_Click: customerCancelButton_Click,
    //    CustomerSaveButton_Click: customerSaveButton_Click,
    //    EmployeeEditPopup_EndCallback: employeeEditPopup_EndCallback,
    //    EvaluationEditPopup_EndCallback: evaluationEditPopup_EndCallback,
    //    TaskEditPopup_EndCallback: taskEditPopup_EndCallback,
    //    CustomerEditPopup_EndCallback: customerEditPopup_EndCallback
    };
window.DXUploadedFilesContainer = DXUploadedFilesContainer;
})();
