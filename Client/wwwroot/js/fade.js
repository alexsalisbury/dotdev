function screenToSVG(screenX, screenY) {
	const svg = document.getElementById("svgFade");
	var p = svg.createSVGPoint();
	p.x = screenX;
	p.y = screenY;
	var ctx = svg.getScreenCTM().inverse();
	var k = p.matrixTransform(ctx);
	var res = k.x + "_" + k.y;
	return res;
}