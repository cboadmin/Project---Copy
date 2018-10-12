var sync = Sync;
var userid = 0;
function InitializeSync() {
	sync.ajax(
		{
			url: 'wslogin.brgybudgetdvo.com', //localhost:33016 // wslogin.brgybudgetdvo.com
			username: "cbodaryll@brgyBudgetdvo.com",
			password: "CBOAdmin",
			dataarival: function (data) {
				if (data.includes('brgyuser.spUserprofileGet')) {
					var o = JSON.parse(data)
					var rec = o['brgyuser.spUserprofileGet'];
					$("#iduserprofilebody").html('');
					$("#tmpuserprofile").tmpl(eval(rec)).appendTo("#iduserprofilebody");
				}
				if (data.includes('brgyuser.spUserprofileDelete')) {
					sync.send('procedurename=brgyuser.spUserprofileGet');
				}
				if (data.includes('brgyuser.spUserprofileUpdate')) {
					sync.send('procedurename=brgyuser.spUserprofileGet');
				}
			},
			success: function (data) {
				console.log(data);
				sync.send('procedurename=brgyuser.spUserprofileGet');
			},
			error: function (data) {
				console.log(data);
			}
		});
}

$(window).ready(function () {


	var sync = Sync;
	sync.ajax(
		{
			url: 'BBS.brgybudgetdvo.com',//'BBS.brgybudgetdvo.com' // localhost:33016
			username: 'arthesaints@gmail.com',
			password: 'Mercury3356',
			success: function (data) {
				console.log(data);
			},
			error: function (data) {
				console.log(data);
			},
			dataarival: function (data) {
				console.log(data);
				sync.send("hello");

			}
		});

	



});