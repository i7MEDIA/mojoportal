/** 2023OCT06
 * NeatHtml has been in ~/ClientScript/NeatHtml.js and in ~/NeatHtml/NeatHtml.js
 * It has been moved to only ~/NeatHtml/NeatHtml.js (by build event on the NeathHtml project
 * This script is here to prevent issues for plugins which reference this location. 
 */
!function () { if (!windows.NeatHtml) { const t = document.currentScript, e = document.createElement("script"); e.src = location.origin + "/NeatHtml/NeatHtml.js", t.nextSibling && t.parentNode.insertBefore(e, t.nextSibling) } }();