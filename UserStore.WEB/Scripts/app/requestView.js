//$(document).ready(function () {
function updateRequest(id,here) {
    var checked = here.checked;
    //$('input[name=Scanned]').click(function (e) {
        //    e.preventDefault();
        //  //  var name = this.val();
        //    name = encodeURIComponent(name);
        //$('#results').load('@Url.Action("Index", "Request")?name=' + name)
        ////});
    var request = { id: id, scanned: checked };
    $.ajax({
        method: 'post',
        url: "Index",
        data: JSON.stringify(request),
        contentType: "application/json; charset=utf-8",
        headers: {
        },
        success: function (response) {
        
        }
    });
    }
//});