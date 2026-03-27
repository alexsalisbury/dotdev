let feelsTimer = undefined;

export function stopFeels() {
    clearInterval(feelsTimer);
}

export function consolePrint(...lines) {
    function stepAppend() {
        let t = document.getElementById("consoleTarget");
        if (!t) {
            return;
        }

        var text = lines[line];
        var prev = lines.slice(0, line).join('&lt;br /&gt;');
        var lineNotDone = idx <= text.length;
        var sub = text.slice(0, idx);

        if (lineNotDone) {
            idx = idx + 1;
            t.innerHTML = prev + '&lt;br /&gt;' + sub + '_';
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
        const t = document.getElementById("consoleTarget");
        if (!t) {
            expire();
            return;
        }

        if (expiry === 0) {
            clearInterval(feelsTimer);
        }

        if (expiry > 0) {
            const under = expiry % 2 === 0 ? '_' : '';
            t.innerHTML = lines.join('&lt;br /&gt;') + under;
            expiry--;
        }
    }

    function expire() {
        expiry = 0;
    }

    clearInterval(feelsTimer);
    let expiry = 1000;
    let line = 0;
    let idx = 0;

    feelsTimer = setInterval(stepAppend, 80);
}