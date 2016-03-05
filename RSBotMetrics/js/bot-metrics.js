'use strict';

$(document).ready(function () {
	displayXpGainedTable();
});

var displayXpGainedTable = function () {
	$.ajax({
		url: '/api/skills',
		type: 'GET',
		success: function (data) {
			var totalXp = 0;
			var xpTable1 = $('#xp-table-1');
			var xpTable2 = $('#xp-table-2');

			var thead1 = xpTable1.find('thead');
			var tbody1 = xpTable1.find('tbody');
			var thead2 = xpTable2.find('thead');
			var tbody2 = xpTable2.find('tbody');

			thead1.append('<tr>\
				<th>Skill</th>\
				<th>XP</th>\
				</tr>');

			thead2.append('<tr>\
				<th>Skill</th>\
				<th>XP</th>\
				</tr>');

			for (var i = 0; i < Math.ceil(data.length / 2); i++) {
				tbody1.append('<tr>\
					<td>' + data[i].name + '</td>\
					<td>' + data[i].xp.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</td>\
					</tr>');
				totalXp += data[i].xp;
			}

			for (var i = Math.ceil(data.length / 2); i < data.length; i++) {
				tbody2.append('<tr>\
					<td>' + data[i].name + '</td>\
					<td>' + data[i].xp.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</td>\
					</tr>');
				totalXp += data[i].xp;
			}

			tbody2.append('<tr>\
				<td>Total</td>\
				<td>' + totalXp.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</td>\
				</tr>');
		},
		error: function (data) {
			console.log(data);
		}
	});
};