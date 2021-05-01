var systemNotifications = {};

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: "hover", placement: "top"
    });

    $('#table').DataTable({
        "ordering": false,
        "info": false
    });

    $('#table-sent-message').DataTable({
        "ordering": false,
        "info": false
    });

    $('#table-reply-message').DataTable({
        "ordering": false,
        "info": false
    });

    $('.date-mask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/aaaa' });

    $('input[type = submit]').click(function () {
        if ($("#form-id").valid()) {
            $('.spinner').css('display', 'block');
        }
    });
});

$(function () {

    var totalNotifications = 0;

    $("#table").on('click', '#Delete', function () {
        var id = $(this).attr("name");
        var controller = $(this).attr("value");

        swal({
            title: "Pretende continuar?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/' + controller + '/Delete',
                    type: "POST",
                    dataType: "JSON",
                    data: { Id: id },
                    success: function (DeleteResponseMessage) {
                        if (DeleteResponseMessage.sucesso === true) {
                            swal(DeleteResponseMessage.message)
                                .then((value) => {
                                    location.reload();
                                });
                        } else {
                            swal(DeleteResponseMessage.message);
                        }
                    }
                });
                setTimeout(
                    function () {
                        location.reload();
                        $('.spinner').css('display', 'block');
                    }, 2000);
            }

        });
    });

    $("#table-reply-message").on('click', '#viewmore', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Messages/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (MessageViewModel) {
                swal(MessageViewModel.text);
            }
        })
    });

    $("#table-reply-message").on('click', '.sent-msg', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Messages/EditPost',
            type: "POST",
            dataType: "JSON",
            data: { Id: id },
            success: function () {
            }
        })
        setTimeout(
            function () {
                location.reload();
                $('.spinner').css('display', 'block');
            }, 2000);
    });

    $("#table-sent-message").on('click', '#viewmore', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Messages/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (MessageViewModel) {
                swal(MessageViewModel.text);
            }
        })
    });

    //Bind Actions on DropDow
    $("#ControllerList").change(function () {
        var controllerId = $("#ControllerList").val();
        var controllerName = $("#ControllerList option:selected").text();
        var userId = $("#UserId").val();
        $("#ControllerName").val($("#ControllerList option:selected").text());

        var url = "/ManageUserClaims/GetActionsList/";
        $.getJSON(url, { controllerId: controllerId, userId: userId, controllerName: controllerName }, function (data) {
            var item = "";
            $("#ActionList").empty();
            $.each(data, function (i, action) {
                item += '<option value="' + action.value + '">' + action.text + '</option>'
            });
            $("#ActionList").html(item);
        });
    });


    $("#table").on('click', '#viewmore-careplan', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/CarePlans/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.carePlan.description);
            }
        })
    });

    createNotificationItem = function (_n) {
        if (_n == null)
            return;

        if (_n.key == 'total')
            return;

        if (_n.labelFormatted == null || "" == _n.labelFormatted)
            return;

        var separator = '<div class="dropdown-divider"></div>';
        var linkbar = '<a href="' + _n.actionUrl + '" class="dropdown-item notificationFor' + _n.key + '"><i class="fas ' + _n.icon + ' mr-2"></i> <span class="">' + _n.labelFormatted + '</span><span class="float-right text-muted text-sm"></span></a>';

        $(".divNotifications").append(separator + linkbar);
    }

    updateNotifications = function () {
        systemNotifications.forEach(function (item) {
            createNotificationItem(item);
        });

        if ($(".totalNotifications") != null) {
            $(".totalNotifications").html(systemNotifications.find(x => x.key === 'total').value);
            $(".totalNotificationsLabel").html(systemNotifications.find(x => x.key === 'total').labelFormatted);
        }
        if ($(".topWarnings") != null) {
            $.each(systemNotifications.find(x => x.key === 'warnings').value, function (i, val) {
                var icon = ' <i style="color:red;" class="bi bi-exclamation-triangle"></i>';
                $(".topWarnings").html(val + icon);
            });
        }

    }

    getNotifications = function () {
        $.ajax({
            url: "/Notifications/",
            type: "GET",
            success: function (data) {
                systemNotifications = data;
                updateNotifications();
            },
            error: function (ex) {
                console.log(ex);
                systemNotifications = {};
            }
        }); //endajax
    }

    getNotifications();

    $("#table-paged").on('click', '#Delete', function () {
        var id = $(this).attr("name");
        var controller = $(this).attr("value");

        swal({
            title: "Pretende continuar?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/' + controller + '/Delete',
                    type: "POST",
                    dataType: "JSON",
                    data: { Id: id },
                    success: function (DeleteResponseMessage) {
                        if (DeleteResponseMessage.sucesso === true) {
                            swal(DeleteResponseMessage.message)
                                .then((value) => {
                                    location.reload();
                                });
                        } else {
                            swal(DeleteResponseMessage.message);
                        }
                    }
                });
                setTimeout(
                    function () {
                        location.reload();
                    }, 2000);
            }

        });
    });
});