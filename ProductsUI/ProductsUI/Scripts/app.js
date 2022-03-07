
$(document).ready(function () {

    var CategoryData = {};
    var ProductData = {};

    if (productId != '') {
        $('#btnSubmit').hide();
        FetchProductData();
    }
    else {
        $('#btnEdit').hide();
        FetchCategoryData();
    }

    function FetchCategoryData() {
        $.ajax({
            method: 'GET',
            dataType: 'json',
            url: '/Home/GetCategoriesDetails',
            data: null,
            success: function (data) {
                CategoryData = data;
                for (item of CategoryData) {
                    $('#categorydd').append($(document.createElement('option')).prop({
                        value: item.Id,
                        text: item.Name
                    }))
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('#msg').text('Error in getting category details. Plese try again or contact helpdesk.').css('color', '#FF0000');
                console.log(errorThrown);
            }
        });
    }    

    function FetchProductData() {
        $.ajax({
            method: 'GET',
            dataType: 'json',
            url: '/Home/GetProductDetails',
            data: { 'id': productId },
            success: function (data) {
                ProductData = data;
                BindProductDataToControls(ProductData);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('#msg').text('Error in getting product details. Plese try again or contact helpdesk.').css('color', '#FF0000');
                console.log(errorThrown);
            }
        });
    }

    $("#categorydd").change(function () {
        $('div[id^="attrdiv"]').remove();
        for (item of CategoryData) {
            if (item.Id == this.value) {
                for (attr of item.Attributes) {
                    var newRowDiv = $(document.createElement('div')).prop({ id: 'attrdiv' + attr.AttributeId, class: 'form-group' });
                    newRowDiv.after().html('<label class="control-label col-md-2">' + attr.Name + '</label><div class="col-md-10"><input type="text" class="form-control" id="txt' + attr.AttributeId + '" name="txt' + attr.AttributeId + '" /></div>');
                    newRowDiv.appendTo("#catAttr");
                }
            }
        }
    });

    function BindProductDataToControls(productData) {
        $('#pName').val(productData.Name);
        $('#pDesc').val(productData.Description);
        $('#categorydd').append($(document.createElement('option')).prop({
            value: productData.Category.Id,
            text: productData.Category.Name,
            selected: true            
        }));
        $('#categorydd').prop({ disabled: true, title: 'Product category cannot be changed' });

        //create text boxes and populate values
        for (item of productData.Attributes) {
            for (attr of productData.Category.Attributes) {
                if (item.Id == attr.AttributeId) {
                    var newRowDiv = $(document.createElement('div')).prop({ id: 'attrdiv' + attr.AttributeId, class: 'form-group' });
                    newRowDiv.after().html('<label class="control-label col-md-2">' + attr.Name + '</label><div class="col-md-10"><input type="text" class="form-control" id="txt' + attr.AttributeId + '" name="txt' + attr.AttributeId + '" value="' + item.Value + '" /></div>');
                    newRowDiv.appendTo("#catAttr");
                }
            }
        }
    }

    $("#btnSubmit").click(function () {
        var validation = ValidateInputs();
        if (validation == true) {
            var createProductRequest = {
                'Name': $('#pName').val(),
                'Description': $('#pDesc').val(),
                'Category': {
                    'Id': $("#categorydd").val()
                },
                'Attributes': []
            };

            for (item of CategoryData) {
                for (attr of item.Attributes) {
                    if ($('#txt' + attr.AttributeId) != undefined && $('#txt' + attr.AttributeId).val() != undefined) {
                        var prodAttr = {};
                        prodAttr.Id = attr.AttributeId;
                        prodAttr.Value = $('#txt' + attr.AttributeId).val();
                        createProductRequest.Attributes.push(prodAttr);
                    }
                }
            }

            $.ajax({
                method: 'POST',
                dataType: 'json',
                url: '/Home/Create',
                data: createProductRequest,
                success: function (data) {
                    if (data.Id != undefined)
                        $('#msg').text('Product added successfully.').css('color', '#006400');
                    else
                        $('#msg').text('Something went wrong. Plese try again or contact helpdesk.').css('color', '#FF0000');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#msg').text('Error in adding product. Plese try again or contact helpdesk.').css('color', '#FF0000');
                    console.log(errorThrown);
                }
            });
        }
    });

    function ValidateInputs() {
        $('#msg').text('');
        let validateResult = true;
        let errors = '';
        if ($('#pName').val() == '' || $('#pName').val() == undefined) {
            errors = errors + 'Name cannot be blank.';
            validateResult = false;
        }
        if ($('#pDesc').val() == '' || $('#pDesc').val() == undefined) {
            errors = errors + ' Description cannot be blank.';
            validateResult = false;
        }
        if ($('#categorydd').val() == 0 || $('#categorydd').val() == undefined) {
            errors = errors + ' Category cannot be blank, please select an option.';
            validateResult = false;
        }
        if (validateResult == false) {
            $('#msg').text(errors).css('color', '#FF0000');
        }
        return validateResult;
    }

    $("#btnEdit").click(function () {
        var validation = ValidateInputs();
        if (validation == true) {
            var editProductRequest = {
                'Id': productId,
                'Name': $('#pName').val(),
                'Description': $('#pDesc').val(),
                'Attributes': []
            };

            for (item of ProductData.Attributes) {
                if ($('#txt' + item.Id) != undefined && $('#txt' + item.Id).val() != undefined) {
                    var prodAttr = {};
                    prodAttr.Id = item.Id;
                    prodAttr.Value = $('#txt' + item.Id).val();
                    editProductRequest.Attributes.push(prodAttr);
                }
            }

            $.ajax({
                method: 'POST',
                dataType: 'json',
                url: '/Home/Edit',
                data: editProductRequest,
                success: function (data) {
                    if (data.Id == productId)
                        $('#msg').text('Product edited successfully.').css('color', '#006400');
                    else
                        $('#msg').text('Something went wrong. Plese try again or contact helpdesk.').css('color', '#FF0000');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#msg').text('Error in editing product. Plese try again or contact helpdesk.').css('color', '#FF0000');
                    console.log(errorThrown);
                }
            });
        }
    });
});