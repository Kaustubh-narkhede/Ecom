app.controller('MainController', function ($scope) {
    $scope.message = "Welcome to the Admin Dashboard!";
});

app.controller('CategoryMaster', function ($scope, $http) {
    $scope.message = "Category Master Page Loaded! Shubbham";
    $scope.categories = [];
    $scope.GetData = function () {
        var requestData = {
            FilterParameter: JSON.stringify({
                ActionId: "getcategory",
                Pid: 0,
                json_text: JSON.stringify({
                    SubCatId: 0,
                    CatName: $scope.categoryname,
                    Description: $scope.description,
                    Deleted: false,
                    SortOrder: $scope.sortorder
                })
            })
        };

        $http.post("/api/CommonAPI/CategoryMaster", requestData)
            .then(function (response) {
                $scope.categories = response.data;

            })
            .catch(function (error) {
                alert("Error while adding category.");
                console.error(error);
            });

    }
    $scope.GetData();

    $scope.AddCategory = function () {
        var requestData = {
            FilterParameter: JSON.stringify({
                ActionId: "insertcategory",
                Pid: 0,
                json_text: JSON.stringify({
                    SubCatId: 0,
                    CatName: $scope.categoryname,
                    Description: $scope.description,
                    Deleted: false,
                    SortOrder: $scope.sortorder
                })
            })
        };

        $http.post("/api/CommonAPI/CategoryMaster", requestData)
            .then(function (response) {
                alert("Category added successfully!");
                $("#addCategoryModal").modal('hide');
                console.log(response.data);
                $scope.GetData();
            })
            .catch(function (error) {
                alert("Error while adding category.");
                console.error(error);
            });

    }

    $scope.EditParentCat = function (data) {
        $("#addCategoryModal").modal('show');

        $scope.categoryname = data.CatName;
        $scope.description = data.Description;
        $scope.sortorder = ESLint(data.sortorder);
    }
    $scope.DetetepParentCat = function (data) {



    }
});




app.controller('UserMaster', function ($scope, $http) {
    $scope.message = "Category Master Page Loaded! Kaustubh";
    $scope.User = [];
    $scope.ActionId = "";
    $scope.UId = 0;

    $scope.AddUser = function () {
        $("#addUserModal").modal('show');
        $scope.ActionId ='insertuser'
    }

    $scope.GetData = function () {

        var requestData = {
            FilterParameter: JSON.stringify({
                ActionId: 'getUser',
                Uid: 0,
                json_text: ""
            })
        };

        $http.post("/api/CommonAPI/UserMaster", requestData)
            .then(function (response) {
                $scope.User = response.data;

            })
            .catch(function (error) {
                alert("Error while adding User.");
                console.error(error);
            });

    }
    $scope.GetData();


    $scope.SaveUser = function () {

        var requestData = {
            FilterParameter: JSON.stringify({
                ActionId: $scope.ActionId,
                Uid: $scope.UId,
                json_text: JSON.stringify({
                    FirstName: $scope.UserFirstName,
                    LastName: $scope.UserLastName,
                    EmailId: $scope.UserEmailid,
                    MobileNo: $scope.UserMobileNo,
                    Add1: $scope.UserAddress,
                    Add2: $scope.UserAddress1,
                    Country: $scope.UserCountry,
                    state: $scope.Userstate,
                    City: $scope.UserCity,
                    Pincode: $scope.UserPincode,
                    Sortorder: $scope.Usersortorder,
                    //Deleted: false

                })
            })
        };

        $http.post("/api/CommonAPI/UserMaster", requestData)
            .then(function (response) {
                alert("User added successfully!");
                $("#addUserModal").modal('hide');
                console.log(response.data);
                $scope.GetData();
            })
            .catch(function (error) {
                alert("Error while adding User.");
                console.error(error);
            });

    }
    $scope.EditUser = function (data) {
        $("#addUserModal").modal('show');
        $scope.ActionId = 'updateuser';
        $scope.UserFirstName = data.FirstName;
        $scope.UserLastName = data.LastName;
        $scope.UserEmailid = data.EmailId;
        $scope.UserMobileNo = data.MobileNo;
        $scope.UserAddress = data.Add1;
        $scope.UserAddress1 = data.Add2;
        $scope.UserPincode = data.Pincode;
        $scope.UserCountry = data.Country;
        $scope.UId = data.Uid;


        $scope.Usersortorder = ESLint(data.Sortorder);
    }

    $scope.DeleteUser = function (data) {

        var requestData = {
            FilterParameter: JSON.stringify({
                ActionId: "DeleteUser",
                Uid: data.Uid
            })
        };

        $http.post("/api/CommonAPI/UserMaster", requestData)
            .then(function (response) {
                alert("User deleted successfully!");
                $scope.GetData();
            })
            .catch(function (error) {
                alert("Error while deleting user.");
                console.error(error);
            });

    }


    $scope.ToggleUserStatus = function (data) {
        var statusText = data.IsActive === 1 ? "Deactivate" : "Activate";

        if (confirm("Are you sure you want to " + statusText + " this user?")) {
            var requestData = {
                FilterParameter: JSON.stringify({
                    ActionId: "Activedeactive",
                    Uid: data.Uid // Send correct user ID
                })
            };

            $http.post("/api/CommonAPI/UserMaster", requestData)
                .then(function (response) {
                    alert("User " + statusText + "d successfully!");
                    data.IsActive = data.IsActive === 1 ? 0 : 1; // Toggle status in UI
                })
                .catch(function (error) {
                    alert("Error while updating user status.");
                    console.error(error);
                });
        }
    };




});







app.controller('Product', function ($scope, $http) {
    $scope.message = "Products Master Page Loaded! Kaustubh";
    $scope.Product = [];
    $scope.ActionId = "";
    $scope.PID = 0;
   

    $scope.AddUser = function () {
        $("#addProduct").modal('show');
        $scope.ActionId = 'Insertproduct'
    }

    $scope.GetData = function () {

        var requestData = {
            FilterParameter: JSON.stringify({
                ActionId: 'getproduct',
                PID: 0,
                json_text: ""
            })
        };

        $http.post("/api/CommonAPI/Products", requestData)
            .then(function (response) {
                $scope.Product = response.data;

            })
            .catch(function (error) {
                alert("Error while adding Product.");
                console.error(error);
            });

    }
    $scope.GetData();


    $scope.SaveProduct = function () {

        var exchangeValue = $scope.Exchangeable ? 2 : 1;
        var RefundableValue = $scope.Refundable ? 2 : 1;

        var requestData = {
            FilterParameter: JSON.stringify({
                ActionId: $scope.ActionId,
                PID: $scope.PID,
                json_text: JSON.stringify({
                    Product_Name: $scope.ProductName,
                    Product_Type: $scope.ProductType,
                    Category_id: $scope.ddlcategory,
                    Subcategory: $scope.ddlSubcategory,
                    Brand: $scope.ddlBrand,
                    Unit: $scope.ddlUnits,
                    Exchangeable: exchangeValue,
                    Refundable: RefundableValue,
                    Product_Description: $scope.Description,
                   
                    //Deleted: false

                })
            })
        };

        $http.post("/api/CommonAPI/Products", requestData)
            .then(function (response) {
                alert("Product added successfully!");
                $("#addProduct").modal('hide');
                console.log(response.data);
                $scope.GetData();
            })
            .catch(function (error) {
                alert("Error while adding Product.");
                console.error(error);
            });

    }


    $scope.GetCategory = function () {

        var requestData = {
            FilterParameter: JSON.stringify({
                pid: 0,
                ActionId: "ddlCategoryMaster"
            })
        };

        $http.post("/api/CommonAPI/Products", requestData)
            .then(function (response) {

                console.log(response.data);
                $scope.CategoryList = response.data;

            })
            .catch(function (error) {

                alert("Error while loading Category ddl.");
                console.error(error);

            });
    };

    $scope.GetCategory();

});