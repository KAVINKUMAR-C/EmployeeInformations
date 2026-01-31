
function PageUtil() {

}

PageUtil.goTo = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-modal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-modal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToTab = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#tab-modal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#tab-modal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToViewEmployee = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-employee-viewmodel").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-employee-viewmodel").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToClient = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-clientmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-clientmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToHelpdesk = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-Helpdeskmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-Helpdeskmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToTicketType = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-tickettypemodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-tickettypemodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToProject = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-projectmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-projectmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToTimeSheet = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-TimeSheetmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-TimeSheetmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToTeamsMeeting = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-TeamsMeetingmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-TeamsMeetingmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}




PageUtil.goToMailScheduler = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-MailSchedulermodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-MailSchedulermodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToContent = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#page-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#page-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToPasswordContent = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#page_password_content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#page_password_content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToLeaveContent = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-leavemodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-leavemodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToWebsiteJobPostContent = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-websitejobpost-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-websitejobpost-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}


PageUtil.goToPermission = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-rolemodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-rolemodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}



PageUtil.goToAllAssets = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-AllAssetsmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-AllAssetsmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToAllExpenses = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-Expensesmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-Expensesmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToCompany = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-Company-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-Company-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToProjectAssignation = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-projectAssignationmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-projectAssignationmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToEmailDraftContent = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-EmailDraftContentmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-EmailDraftContentmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToWorkFromHome = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-WorkFromHome-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-WorkFromHome-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToPermission = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-rolemodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-rolemodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToDashboardPermission = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-dashboardrolemodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-dashboardrolemodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToEmployeeSetting = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-companysettingmodal-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-companysettingmodal-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}

PageUtil.goToWebsiteAppliedJobContent = function (srvUrl, callback) {
    $.ajax({
        url: srvUrl,
        success: function (result) {
            $('.loader').addClass('d-none');
            $("#bind-websiteappliedjob-content").html(result);
        },
        beforeSend: function () {
            $('.loader').removeClass('d-none');
            $("#bind-websiteappliedjob-content").html("");
        },
        error: function (xhr) {
            $('.loader').addClass('d-none');
            window.location.href = xhr.responseJson.url;
        }
    });
    return false;
}