//function updateRequest() {
    
//    $('input[name=Scanned]').click(function (e) {
//            e.preventDefault();
//            var name = this.val();
//            name = encodeURIComponent(name);
//        $('#results').load('@Url.Action("Index", "Request")?name=' + name)
//        });
   


//    $.ajax({
//        method: 'post',
//        url: "Request/Index",
//        data: JSON.stringify(transaction),
//        contentType: "application/json; charset=utf-8",
//        headers: {
//            'Authorization': 'Bearer ' + app.dataModel.getAccessToken()
//        },
//        success: function (response) {
//            $scope.transactions.unshift(response);
//            $scope.currentBalance = $scope.currentBalance - response.sum;
//            $scope.$apply();
//        }
//    });
//}