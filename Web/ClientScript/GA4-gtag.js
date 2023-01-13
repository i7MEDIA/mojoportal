var urlParams = new URLSearchParams(document.currentScript.src.substring(document.currentScript.src.indexOf('?')));
window.dataLayer = window.dataLayer || [];
function gtag() { dataLayer.push(arguments); }
gtag('js', new Date());
gtag('config', urlParams.get('id'));