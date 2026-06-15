'use strict';

(function () {
	// Select all range inputs
	var ranges = document.querySelectorAll('.range-w-output');

	var _loop = function _loop(i) {
		var range = ranges[i],
			input = range.querySelector('.range-w-output__range'),
			output = range.querySelector('.range-w-output__output');

		// On init
		// Add as many zeros to the output that the max on the input allows
		for (var j = 0; input.max.length > j; j++) {
			output.innerHTML += 0;
		}

		// Detect the width of the output and set that width on it
		output.style.width = window.getComputedStyle(output).getPropertyValue('width');

		// Set the proper value on output
		output.innerHTML = input.value;

		// On input | All broweser except IE
		input.addEventListener('input', function (e) {
			output.innerHTML = input.value;
		});

		// On change | Just for IE
		input.addEventListener('change', function (e) {
			output.innerHTML = input.value;
		});
	};

	// Loop through ranges
	for (var i = 0; ranges.length > i; i++) {
		_loop(i);
	}
})();