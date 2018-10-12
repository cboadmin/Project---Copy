// License:
//
// Copyright (c) 2018-2020, Arthur Bryan Santos
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

$(function () {

	//-- validate if email
	$('#frmlogin').submit(function (event) {
		event.preventDefault();
		submitLogin();
	});
});

function submitLogin() {


	$.post(baseUrl() + "api/Login", { 'Username': $('#email').val(), 'Password': $('#password').val(), 'key': generalKey() }, function (data) {
		// console.log(data);
		$('#password').val('');
		if (data.ValidateUser != '' && data.key != undefined && data.key != '') {
			var buildParam = '?appkey=' + data.key + "&email=" + $('#email').val() + "&uid=" + data.ValidateUser[0].GenealogyID + "&tok=" +
				data.ValidateUser[0].Code + '&ReturnUrl=' + getUrlParameter('ReturnUrl');
			window.location.replace(baseUrl() + data.url + buildParam);
		} else {
			$('#iloader').hide();
			$("#errorMsg").show().delay(5000).fadeOut();
		}


	}, 'json');
}
