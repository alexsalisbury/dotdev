// Lookit me just blantantly dropping all this on window. Shame. 🔔
let feelsTimer = undefined;   

function stopFeels() {
    clearInterval(feelsTimer);
}

function consoleQuickPrint(...lines) {
    clearInterval(feelsTimer);
    let t = document.getElementById("consoleTarget");
    let expiry = 1000;
    let line = 0;
    let idx = 0;

    function stepAppend() {
        var text = lines[line];
        var prev = lines.slice(0, line).join('<br />');
        var next = idx <= text.length;
        var sub = text.slice(0, idx);
        if (next) {
            idx = idx + 1;
            console.log(sub);
            t.innerHTML = prev + '<br />' + sub + '_';
        } else {
            if (line < lines.length - 1) {
                idx = 0;
                line += 1;
            } else {
                clearInterval(feelsTimer);
            }
        }
    }

    feelsTimer = setInterval(stepAppend, 80);
}

function consoleBlink(expiry = 1000) {
    clearInterval(feelsTimer);
    let existing = t.innerHTML;

    function blink() {
        if (expiry === 0) {
            clearInterval(feelsTimer);
        }
        if (expiry > 0) {
            var under = (expiry % 2 === 1) ? '_' : ''
            t.innerHTML = existing + under;
            console.log(expiry);
            expiry = expiry - 1;
        }
    }

    feelsTimer = setInterval(blink, 80);
}

function consolePrint(...lines) {
    let t = document.getElementById("consoleTarget");
    clearInterval(feelsTimer);
    let expiry = 1000;
    let line = 0;
    let idx = 0;

    function stepAppend() {
        var text = lines[line];
        var prev = lines.slice(0, line).join('<br />');
        var next = idx <= text.length;
        var sub = text.slice(0, idx);
        if (next) {
            idx = idx + 1;
            console.log(sub);
            t.innerHTML = prev + '<br />' + sub + '_';
        } else {
            if (line < lines.length - 1) {
                idx = 0;
                line += 1;
            } else {
                clearInterval(feelsTimer);
                feelsTimer = setInterval(blink, 1000);
            }
        }
    }

    function blink() {
        if (expiry === 0) {
            clearInterval(feelsTimer);
        }
        if (expiry > 0) {
            var under = (expiry % 2 === 0) ? '_' : ''
            t.innerHTML = lines.join('<br />') + under; // I should probably memoize here.
            console.log(expiry);
            expiry = expiry - 1;
        }
    }

    function expire() {
        expiry == 0;
    }

    feelsTimer = setInterval(stepAppend, 80);
}