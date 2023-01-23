/*****
 * 
 * can be used by any script needing a pub-sub model
 * 
 */

(function(){
	function noop() {}
	function safe_not_equal(a, b) {
		return a != a ? b == b : a !== b || ((a && typeof a === 'object') || typeof a === 'function');
	}
	
	const subscriber_queue = [];
	
	function writable(value, start = noop) {
		let stop;
		const subscribers = new Set();
	
		function set(new_value) {
			if (safe_not_equal(value, new_value)) {
				value = new_value;
	
				if (stop) {
					const run_queue = !subscriber_queue.length;
	
					for (const subscriber of subscribers) {
						subscriber[1]();
						subscriber_queue.push(subscriber, value);
					}
	
					if (run_queue) {
						for (let i = 0; i < subscriber_queue.length; i += 2) {
							subscriber_queue[i][0](subscriber_queue[i + 1]);
						}
	
						subscriber_queue.length = 0;
					}
				}
			}
		}
	
		function update(fn) {
			set(fn(value));
		}
	
		function subscribe(run, invalidate = noop) {
			const subscriber = [run, invalidate];
	
			subscribers.add(subscriber);
	
			if (subscribers.size === 1) {
				stop = start(set) || noop;
			}
	
			run(value);
	
			return () => {
				subscribers.delete(subscriber);
	
				if (subscribers.size === 0) {
					stop();
					stop = null;
				}
			};
		}
	
		return { set, update, subscribe };
	}
	window.mojopubsub = writable;
})();
