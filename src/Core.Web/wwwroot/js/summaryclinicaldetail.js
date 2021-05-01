$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip({
        trigger: "hover", placement: "top"
    });

    $('table.display').DataTable({
        "ordering": false,
        "info": false
    });

    $('#facilitator-pathology-allergy').hide();
    $('#facilitator-name-allergy').hide();

    $('#facilitator-pathology-diagnostic').hide();
    $('#facilitator-name-diagnostic').hide();

    $('#facilitator-pathology-observation').hide();
    $('#facilitator-name-observation').hide();

    $("#form-id-message").on('click', 'input[type = submit]', function () {
        if ($("#form-id-message").valid()) {
            $('.spinner').css('display', 'block');
        }
    });

    $("#form-id-inquiry-schedule").on('click', 'input[type = submit]', function () {
        if ($("#form-id-inquiry-schedule").valid()) {
            $('.spinner').css('display', 'block');
        }
    });

    $("#form-id-observation").on('click', 'input[type = submit]', function () {
        if ($("#form-id-observation").valid()) {
            $('.spinner').css('display', 'block');
        }
    });

    $("#form-id-diagnostic").on('click', 'input[type = submit]', function () {
        if ($("#form-id-diagnostic").valid()) {
            $('.spinner').css('display', 'block');
        }
    });

    $("#form-id-consultation").on('click', 'input[type = submit]', function () {
        if ($("#form-id-consultation").valid()) {
            $('.spinner').css('display', 'block');
        }
    });

    $("#form-id-careplans").on('click', 'input[type = submit]', function () {
        if ($("#form-id-careplans").valid()) {
            $('.spinner').css('display', 'block');
        }
    });

    $("#form-id-allergy").on('click', 'input[type = submit]', function () {
        if ($("#form-id-allergy").valid()) {
            $('.spinner').css('display', 'block');
        }
    });

    $('.date-mask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/aaaa' });
});

$(function () {

    $("#Allergy_ClinicalSummaryFacilitator").change(function () {
        var selected = $("#Allergy_ClinicalSummaryFacilitator option:selected").attr("value");
        if (selected != "") {
            var url = "/SummaryClinicalDetail/GetFacilitatorDescriptionById?id=" + selected;
            $.getJSON(url, function (data) {
                $("#Allergy_Description").text(data);
            });
        }
    });

    $("#Diagnostic_ClinicalSummaryFacilitator").change(function () {
        var selected = $("#Diagnostic_ClinicalSummaryFacilitator option:selected").attr("value");
        if (selected != "") {
            var url = "/SummaryClinicalDetail/GetFacilitatorDescriptionById?id=" + selected;
            $.getJSON(url, function (data) {
                $("#Diagnostic_Description").text(data);
            });
        }
    });

    $("#Observation_ClinicalSummaryFacilitator").change(function () {
        var selected = $("#Observation_ClinicalSummaryFacilitator option:selected").attr("value");
        if (selected != "") {
            var url = "/SummaryClinicalDetail/GetFacilitatorDescriptionById?id=" + selected;
            $.getJSON(url, function (data) {
                $("#Observation_Description").text(data);
            });
        }
    });

    $("#table-consultation").on('click', '#viewmore-consultation', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Consultations/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.consultation.description);
            }
        })
    });

    $("#table-consultation-history-others").on('click', '#viewmore-consultation-others', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Consultations/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.consultation.description);
            }
        })
    });

    $("#table-allergy").on('click', '#viewmore-allergy', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Allergies/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.allergy.description);
            }
        })
    });

    $("#table-allergy-history-others").on('click', '#viewmore-allergy-others', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Allergies/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.allergy.description);
            }
        })
    });

    $("#table-allergy").on('click', '#delete-allergy', function () {
        var id = $(this).attr("data-id");

        swal({
            title: "Pretende continuar?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/Allergies/Delete',
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

    $("#table-diagnostic").on('click', '#viewmore-diagnostic', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Diagnostics/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.diagnostic.description);
            }
        })
    });

    $("#table-diagnostic-history-others").on('click', '#viewmore-diagnostic-others', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Diagnostics/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.diagnostic.description);
            }
        })
    });

    $("#table-diagnostic").on('click', '#delete-diagnostic', function () {
        var id = $(this).attr("data-id");

        swal({
            title: "Pretende continuar?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/Diagnostics/Delete',
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

    $("#table-observation").on('click', '#viewmore-observation', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Observations/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {
                swal(SummaryClinicalDetailViewModel.observation.description);
            }
        })
    });

    $("#table-observation-history-others").on('click', '#viewmore-observation-others', function () {
        var id = $(this).attr("data-id");
        $.ajax({
            url: '/Observations/Detail',
            type: "POST",
            dataType: "JSON",
            data: { id: id },
            success: function (SummaryClinicalDetailViewModel) {                
                swal(SummaryClinicalDetailViewModel.observation.description);
            }
        })
    });

    $("#table-observation").on('click', '#delete-observation', function () {
        var id = $(this).attr("data-id");

        swal({
            title: "Pretende continuar?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/Observations/Delete',
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

    $("#table-careplan").on('click', '#viewmore-careplan', function () {
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

    $("#table-careplan-history-others").on('click', '#viewmore-careplan-others', function () {
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

    $("#table-careplan").on('click', '#delete-careplan', function () {
        var id = $(this).attr("data-id");

        swal({
            title: "Pretende continuar?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    url: '/CarePlans/Delete',
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

    $("#table-diagnostic").on('click', '#clone-diagnostic', function () {
        var id = $(this).attr("data-id");
        var text = $(this).attr("data-text");
        $("#cloneId_diagnostic").val(id);
        $("#Diagnostic_Description").val(text)
    });

    $("#table-observation").on('click', '#clone-observation', function () {
        var id = $(this).attr("data-id");
        var text = $(this).attr("data-text");
        $("#cloneId_observation").val(id);
        $("#Observation_Description").val(text)
    });

    $("#table-careplan").on('click', '#clone-careplan', function () {
        var id = $(this).attr("data-id");
        var text = $(this).attr("data-text");
        $("#cloneId_careplan").val(id);
        $("#CarePlan_Description").val(text)
    });

    $("#isnewfacilitator-allergy").change(function () {
        if (this.checked == true) {
            $('#facilitator-pathology-allergy').show();
            $('#facilitator-name-allergy').show();
        } else {
            $('#facilitator-pathology-allergy').hide();
            $('#facilitator-name-allergy').hide();
        }
        
    });

    $("#isnewfacilitator_diagnostic").change(function () {
        if (this.checked == true) {
            $('#facilitator-pathology-diagnostic').show();
            $('#facilitator-name-diagnostic').show();
        } else {
            $('#facilitator-pathology-diagnostic').hide();
            $('#facilitator-name-diagnostic').hide();
        }

    });

    $("#isnewfacilitator_observation").change(function () {
        if (this.checked == true) {
            $('#facilitator-pathology-observation').show();
            $('#facilitator-name-observation').show();
        } else {
            $('#facilitator-pathology-observation').hide();
            $('#facilitator-name-observation').hide();
        }

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
});