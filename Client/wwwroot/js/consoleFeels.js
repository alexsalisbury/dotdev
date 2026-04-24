let feelsTimer = undefined;
console.log("consoleFeels.js loaded");
export function stopFeels() {
    clearInterval(feelsTimer);
}

export function consolePrint(...lines) {

    console.log("LINES RAW:", lines);
    console.log("TYPE:", typeof lines[0]);
    console.log("CONTENTS CHARACTERS:", [...lines[0]]);

    clearInterval(feelsTimer);

    let t = document.getElementById("consoleTarget");
    if (!t) return;

    let expiry = 1000;
    let line = 0;
    let idx = 0;

    t.innerHTML = "";

    function stepAppend() {
        t = document.getElementById("consoleTarget");
        if (!t) return;

        const current = lines[line];
        const previous = line === 0 ? "" : lines.slice(0, line).join("<br/>");
        const lineNotDone = idx <= current.length;

        if (lineNotDone) {

            const partial = current.slice(0, idx);
            t.innerHTML =
                previous +
                (previous ? "<br/>" : "") +
                partial + "_";

            idx++;

        } else {
            // ✅ FIX: Add newline between completed lines
            if (line < lines.length - 1) {
                t.innerHTML =
                    lines.slice(0, line + 1).join("<br/>") +
                    "<br/>_";

                idx = 0;
                line++;

            } else {
                clearInterval(feelsTimer);
                feelsTimer = setInterval(blink, 600);
            }
        }
    }

    function blink() {
        t = document.getElementById("consoleTarget");
        if (!t) {
            expiry = 0;
            clearInterval(feelsTimer);
            return;
        }

        if (expiry === 0) {
            clearInterval(feelsTimer);
            return;
        }

        const under = (expiry % 2 === 0) ? "_" : "";
        t.innerHTML = lines.join("<br/>") + under;

        expiry--;
    }

    // ✅ Friendlier, readable typing speed
    feelsTimer = setInterval(stepAppend, 45 + Math.random() * 55);
}