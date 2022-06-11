"use strict";
(self["webpackChunkWebUI"] = self["webpackChunkWebUI"] || []).push([["main"],{

/***/ 397:
/*!***************************************************!*\
  !*** ./src/app/_directives/has-role.directive.ts ***!
  \***************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HasRoleDirective": () => (/* binding */ HasRoleDirective)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/account.service */ 7110);



class HasRoleDirective {
    constructor(viewContainerRef, templateRef, accountService) {
        this.viewContainerRef = viewContainerRef;
        this.templateRef = templateRef;
        this.accountService = accountService;
        this.appHasRole = [];
        this.loginResponse = null;
        this.accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_1__.take)(1)).subscribe(l => {
            this.loginResponse = l;
        });
    }
    ngOnInit() {
        var _a;
        // clear the view
        if (this.loginResponse === null || this.loginResponse.userData.roles === null) {
            this.viewContainerRef.clear();
            return;
        }
        if ((_a = this.loginResponse) === null || _a === void 0 ? void 0 : _a.userData) {
            let roles = this.loginResponse.userData.roles;
            if (roles.some(role => this.appHasRole.includes(role))) {
                this.viewContainerRef.createEmbeddedView(this.templateRef);
            }
            else {
                this.viewContainerRef.clear();
                return;
            }
        }
    }
}
HasRoleDirective.ɵfac = function HasRoleDirective_Factory(t) { return new (t || HasRoleDirective)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_2__.ViewContainerRef), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_core__WEBPACK_IMPORTED_MODULE_2__.TemplateRef), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_account_service__WEBPACK_IMPORTED_MODULE_0__.AccountService)); };
HasRoleDirective.ɵdir = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineDirective"]({ type: HasRoleDirective, selectors: [["", "appHasRole", ""]], inputs: { appHasRole: "appHasRole" } });


/***/ }),

/***/ 6299:
/*!****************************************!*\
  !*** ./src/app/_guards/admin.guard.ts ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AdminGuard": () => (/* binding */ AdminGuard)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 635);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/account.service */ 7110);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ngx-toastr */ 4101);




class AdminGuard {
    constructor(accountService, toastrService) {
        this.accountService = accountService;
        this.toastrService = toastrService;
    }
    canActivate() {
        return this.accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_1__.map)(login => {
            if ((login === null || login === void 0 ? void 0 : login.userData.roles.includes('admin')) || (login === null || login === void 0 ? void 0 : login.userData.roles.includes('moderator'))) {
                return true;
            }
            this.toastrService.error("Unauthorized Access");
            return false;
        }));
    }
}
AdminGuard.ɵfac = function AdminGuard_Factory(t) { return new (t || AdminGuard)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](_services_account_service__WEBPACK_IMPORTED_MODULE_0__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_3__.ToastrService)); };
AdminGuard.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineInjectable"]({ token: AdminGuard, factory: AdminGuard.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 1094:
/*!***************************************!*\
  !*** ./src/app/_guards/auth.guard.ts ***!
  \***************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AuthGuard": () => (/* binding */ AuthGuard)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 635);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/account.service */ 7110);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ngx-toastr */ 4101);




class AuthGuard {
    constructor(accountService, toastr) {
        this.accountService = accountService;
        this.toastr = toastr;
    }
    canActivate() {
        return this.accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_1__.map)((response) => {
            if (response) {
                return true;
            }
            else {
                this.toastr.error("You need to login!");
                return false;
            }
        }));
    }
}
AuthGuard.ɵfac = function AuthGuard_Factory(t) { return new (t || AuthGuard)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](_services_account_service__WEBPACK_IMPORTED_MODULE_0__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_3__.ToastrService)); };
AuthGuard.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineInjectable"]({ token: AuthGuard, factory: AuthGuard.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 8225:
/*!**********************************************************!*\
  !*** ./src/app/_guards/prevent-unsaved-changes.guard.ts ***!
  \**********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "PreventUnsavedChangesGuard": () => (/* binding */ PreventUnsavedChangesGuard)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);

class PreventUnsavedChangesGuard {
    canDeactivate(component) {
        if (component.checkIfUserChanged()) {
            return confirm('Are you sure you want to discard the unsaved changes?');
        }
        return true;
    }
}
PreventUnsavedChangesGuard.ɵfac = function PreventUnsavedChangesGuard_Factory(t) { return new (t || PreventUnsavedChangesGuard)(); };
PreventUnsavedChangesGuard.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: PreventUnsavedChangesGuard, factory: PreventUnsavedChangesGuard.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 8971:
/*!****************************************************!*\
  !*** ./src/app/_interceptors/error.interceptor.ts ***!
  \****************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "ErrorInterceptor": () => (/* binding */ ErrorInterceptor)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! rxjs */ 3158);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 5474);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-toastr */ 4101);




class ErrorInterceptor {
    constructor(router, toastr) {
        this.router = router;
        this.toastr = toastr;
    }
    intercept(request, next) {
        return next.handle(request).pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_0__.catchError)((error) => {
            if (error) {
                switch (error.status) {
                    case 400:
                        if (error.error.errors) {
                            const modelStateErrors = [];
                            for (const key in error.error.errors) {
                                modelStateErrors.push(error.error.errors[key]);
                            }
                            throw modelStateErrors.flat();
                        }
                        else if (typeof (error.error) === 'object') {
                            this.toastr.error(error.statusText, error.status);
                        }
                        else {
                            this.toastr.error(error.error, error.status);
                        }
                        break;
                    case 404:
                        this.router.navigateByUrl('not-found');
                        break;
                    case 401:
                        // this.toastr.error(error.statusText, error.status);
                        break;
                    case 500:
                        const navigationExtras = { state: { error: error.error } };
                        this.router.navigateByUrl('/server-error', navigationExtras);
                        break;
                    default:
                        this.toastr.error('something unexpected occured.');
                        break;
                }
            }
            return (0,rxjs__WEBPACK_IMPORTED_MODULE_1__.throwError)(error);
        }));
    }
}
ErrorInterceptor.ɵfac = function ErrorInterceptor_Factory(t) { return new (t || ErrorInterceptor)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](_angular_router__WEBPACK_IMPORTED_MODULE_3__.Router), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_4__.ToastrService)); };
ErrorInterceptor.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineInjectable"]({ token: ErrorInterceptor, factory: ErrorInterceptor.ɵfac });


/***/ }),

/***/ 9707:
/*!**************************************************!*\
  !*** ./src/app/_interceptors/jwt.interceptor.ts ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "JwtInterceptor": () => (/* binding */ JwtInterceptor)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/account.service */ 7110);



class JwtInterceptor {
    constructor(accountService) {
        this.accountService = accountService;
    }
    intercept(request, next) {
        this.accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_1__.take)(1)).subscribe(user => {
            if (user) {
                request = request.clone({
                    setHeaders: {
                        Authorization: `Bearer ${user.token}`
                    }
                });
            }
        });
        return next.handle(request);
    }
}
JwtInterceptor.ɵfac = function JwtInterceptor_Factory(t) { return new (t || JwtInterceptor)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵinject"](_services_account_service__WEBPACK_IMPORTED_MODULE_0__.AccountService)); };
JwtInterceptor.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineInjectable"]({ token: JwtInterceptor, factory: JwtInterceptor.ɵfac });


/***/ }),

/***/ 2046:
/*!******************************************************!*\
  !*** ./src/app/_interceptors/loading.interceptor.ts ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "LoadingInterceptor": () => (/* binding */ LoadingInterceptor)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 1339);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ 2313);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_busy_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/busy.service */ 4109);



class LoadingInterceptor {
    constructor(busyService) {
        this.busyService = busyService;
    }
    intercept(request, next) {
        this.busyService.busy();
        return next.handle(request).pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_1__.delay)(1000), (0,rxjs__WEBPACK_IMPORTED_MODULE_2__.finalize)(() => this.busyService.idle()));
    }
}
LoadingInterceptor.ɵfac = function LoadingInterceptor_Factory(t) { return new (t || LoadingInterceptor)(_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵinject"](_services_busy_service__WEBPACK_IMPORTED_MODULE_0__.BusyService)); };
LoadingInterceptor.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdefineInjectable"]({ token: LoadingInterceptor, factory: LoadingInterceptor.ɵfac });


/***/ }),

/***/ 4057:
/*!***************************************!*\
  !*** ./src/app/_models/userParams.ts ***!
  \***************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserParams": () => (/* binding */ UserParams)
/* harmony export */ });
class UserParams {
    constructor(user) {
        this.minAge = 18;
        this.maxAge = null;
        this.pageNumber = 1;
        this.itemsPerPage = 4;
        this.orderBy = 'lastActive'; // options lastActive, age, creationTime
        this.sex = user.interest;
    }
}


/***/ }),

/***/ 9821:
/*!*******************************************!*\
  !*** ./src/app/_modules/shared.module.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "SharedModule": () => (/* binding */ SharedModule)
/* harmony export */ });
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/tabs */ 2379);
/* harmony import */ var ngx_bootstrap_dropdown__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ngx-bootstrap/dropdown */ 7281);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ngx-toastr */ 4101);
/* harmony import */ var ngx_bootstrap_tabs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-bootstrap/tabs */ 2916);
/* harmony import */ var ng2_file_upload__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ng2-file-upload */ 8356);
/* harmony import */ var ngx_bootstrap_datepicker__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ngx-bootstrap/datepicker */ 1950);
/* harmony import */ var ngx_bootstrap_pagination__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ngx-bootstrap/pagination */ 7977);
/* harmony import */ var ngx_timeago__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ngx-timeago */ 2699);
/* harmony import */ var ngx_bootstrap_buttons__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ngx-bootstrap/buttons */ 6126);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_material_form_field__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @angular/material/form-field */ 9076);
/* harmony import */ var _angular_material_icon__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @angular/material/icon */ 5590);
/* harmony import */ var _angular_material_grid_list__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! @angular/material/grid-list */ 3346);
/* harmony import */ var _angular_material_input__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! @angular/material/input */ 3365);
/* harmony import */ var _angular_material_card__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! @angular/material/card */ 1961);
/* harmony import */ var _angular_material_select__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! @angular/material/select */ 1434);
/* harmony import */ var _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! @angular/material/datepicker */ 5818);
/* harmony import */ var _angular_material_core__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(/*! @angular/material/core */ 8133);
/* harmony import */ var _angular_material_progress_bar__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(/*! @angular/material/progress-bar */ 9782);
/* harmony import */ var _angular_material_table__WEBPACK_IMPORTED_MODULE_21__ = __webpack_require__(/*! @angular/material/table */ 7217);
/* harmony import */ var _angular_material_menu__WEBPACK_IMPORTED_MODULE_22__ = __webpack_require__(/*! @angular/material/menu */ 2796);
/* harmony import */ var _angular_material_dialog__WEBPACK_IMPORTED_MODULE_23__ = __webpack_require__(/*! @angular/material/dialog */ 5758);
/* harmony import */ var _angular_material_checkbox__WEBPACK_IMPORTED_MODULE_24__ = __webpack_require__(/*! @angular/material/checkbox */ 1534);
/* harmony import */ var _angular_material_paginator__WEBPACK_IMPORTED_MODULE_25__ = __webpack_require__(/*! @angular/material/paginator */ 6439);
/* harmony import */ var _angular_material_toolbar__WEBPACK_IMPORTED_MODULE_26__ = __webpack_require__(/*! @angular/material/toolbar */ 9946);
/* harmony import */ var _angular_material_sidenav__WEBPACK_IMPORTED_MODULE_27__ = __webpack_require__(/*! @angular/material/sidenav */ 7216);
/* harmony import */ var _angular_material_progress_spinner__WEBPACK_IMPORTED_MODULE_28__ = __webpack_require__(/*! @angular/material/progress-spinner */ 4742);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);




































class SharedModule {
}
SharedModule.ɵfac = function SharedModule_Factory(t) { return new (t || SharedModule)(); };
SharedModule.ɵmod = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({ type: SharedModule });
SharedModule.ɵinj = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({ imports: [[
            _angular_common__WEBPACK_IMPORTED_MODULE_1__.CommonModule, ngx_bootstrap_dropdown__WEBPACK_IMPORTED_MODULE_2__.BsDropdownModule.forRoot(),
            ngx_toastr__WEBPACK_IMPORTED_MODULE_3__.ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
            ngx_bootstrap_tabs__WEBPACK_IMPORTED_MODULE_4__.TabsModule.forRoot(), ng2_file_upload__WEBPACK_IMPORTED_MODULE_5__.FileUploadModule, ngx_bootstrap_datepicker__WEBPACK_IMPORTED_MODULE_6__.BsDatepickerModule.forRoot(),
            ngx_bootstrap_pagination__WEBPACK_IMPORTED_MODULE_7__.PaginationModule.forRoot(), ngx_timeago__WEBPACK_IMPORTED_MODULE_8__.TimeagoModule.forRoot(), ngx_bootstrap_buttons__WEBPACK_IMPORTED_MODULE_9__.ButtonsModule.forRoot(),
            _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__.MatTabsModule, _angular_material_button__WEBPACK_IMPORTED_MODULE_11__.MatButtonModule,
            _angular_material_form_field__WEBPACK_IMPORTED_MODULE_12__.MatFormFieldModule, _angular_material_icon__WEBPACK_IMPORTED_MODULE_13__.MatIconModule, _angular_material_grid_list__WEBPACK_IMPORTED_MODULE_14__.MatGridListModule, _angular_material_input__WEBPACK_IMPORTED_MODULE_15__.MatInputModule,
            _angular_material_card__WEBPACK_IMPORTED_MODULE_16__.MatCardModule, _angular_material_select__WEBPACK_IMPORTED_MODULE_17__.MatSelectModule, _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_18__.MatDatepickerModule, _angular_material_core__WEBPACK_IMPORTED_MODULE_19__.MatNativeDateModule,
            _angular_material_progress_bar__WEBPACK_IMPORTED_MODULE_20__.MatProgressBarModule, _angular_material_table__WEBPACK_IMPORTED_MODULE_21__.MatTableModule, _angular_material_menu__WEBPACK_IMPORTED_MODULE_22__.MatMenuModule, _angular_material_dialog__WEBPACK_IMPORTED_MODULE_23__.MatDialogModule,
            _angular_material_checkbox__WEBPACK_IMPORTED_MODULE_24__.MatCheckboxModule, _angular_material_paginator__WEBPACK_IMPORTED_MODULE_25__.MatPaginatorModule, _angular_material_toolbar__WEBPACK_IMPORTED_MODULE_26__.MatToolbarModule, _angular_material_sidenav__WEBPACK_IMPORTED_MODULE_27__.MatSidenavModule,
            _angular_material_progress_spinner__WEBPACK_IMPORTED_MODULE_28__.MatProgressSpinnerModule
        ], ngx_bootstrap_dropdown__WEBPACK_IMPORTED_MODULE_2__.BsDropdownModule,
        ngx_toastr__WEBPACK_IMPORTED_MODULE_3__.ToastrModule,
        ngx_bootstrap_tabs__WEBPACK_IMPORTED_MODULE_4__.TabsModule,
        ng2_file_upload__WEBPACK_IMPORTED_MODULE_5__.FileUploadModule,
        ngx_bootstrap_datepicker__WEBPACK_IMPORTED_MODULE_6__.BsDatepickerModule,
        ngx_bootstrap_pagination__WEBPACK_IMPORTED_MODULE_7__.PaginationModule,
        ngx_timeago__WEBPACK_IMPORTED_MODULE_8__.TimeagoModule,
        ngx_bootstrap_buttons__WEBPACK_IMPORTED_MODULE_9__.ButtonsModule,
        _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__.MatTabsModule,
        _angular_material_button__WEBPACK_IMPORTED_MODULE_11__.MatButtonModule,
        _angular_material_form_field__WEBPACK_IMPORTED_MODULE_12__.MatFormFieldModule,
        _angular_material_icon__WEBPACK_IMPORTED_MODULE_13__.MatIconModule,
        _angular_material_grid_list__WEBPACK_IMPORTED_MODULE_14__.MatGridListModule,
        _angular_material_input__WEBPACK_IMPORTED_MODULE_15__.MatInputModule,
        _angular_material_card__WEBPACK_IMPORTED_MODULE_16__.MatCardModule,
        _angular_material_select__WEBPACK_IMPORTED_MODULE_17__.MatSelectModule,
        _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_18__.MatDatepickerModule,
        _angular_material_core__WEBPACK_IMPORTED_MODULE_19__.MatNativeDateModule,
        _angular_material_progress_bar__WEBPACK_IMPORTED_MODULE_20__.MatProgressBarModule,
        _angular_material_table__WEBPACK_IMPORTED_MODULE_21__.MatTableModule,
        _angular_material_menu__WEBPACK_IMPORTED_MODULE_22__.MatMenuModule,
        _angular_material_dialog__WEBPACK_IMPORTED_MODULE_23__.MatDialogModule,
        _angular_material_checkbox__WEBPACK_IMPORTED_MODULE_24__.MatCheckboxModule,
        _angular_material_paginator__WEBPACK_IMPORTED_MODULE_25__.MatPaginatorModule,
        _angular_material_toolbar__WEBPACK_IMPORTED_MODULE_26__.MatToolbarModule,
        _angular_material_sidenav__WEBPACK_IMPORTED_MODULE_27__.MatSidenavModule,
        _angular_material_progress_spinner__WEBPACK_IMPORTED_MODULE_28__.MatProgressSpinnerModule] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](SharedModule, { imports: [_angular_common__WEBPACK_IMPORTED_MODULE_1__.CommonModule, ngx_bootstrap_dropdown__WEBPACK_IMPORTED_MODULE_2__.BsDropdownModule, ngx_toastr__WEBPACK_IMPORTED_MODULE_3__.ToastrModule, ngx_bootstrap_tabs__WEBPACK_IMPORTED_MODULE_4__.TabsModule, ng2_file_upload__WEBPACK_IMPORTED_MODULE_5__.FileUploadModule, ngx_bootstrap_datepicker__WEBPACK_IMPORTED_MODULE_6__.BsDatepickerModule, ngx_bootstrap_pagination__WEBPACK_IMPORTED_MODULE_7__.PaginationModule, ngx_timeago__WEBPACK_IMPORTED_MODULE_8__.TimeagoModule, ngx_bootstrap_buttons__WEBPACK_IMPORTED_MODULE_9__.ButtonsModule, _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__.MatTabsModule, _angular_material_button__WEBPACK_IMPORTED_MODULE_11__.MatButtonModule,
        _angular_material_form_field__WEBPACK_IMPORTED_MODULE_12__.MatFormFieldModule, _angular_material_icon__WEBPACK_IMPORTED_MODULE_13__.MatIconModule, _angular_material_grid_list__WEBPACK_IMPORTED_MODULE_14__.MatGridListModule, _angular_material_input__WEBPACK_IMPORTED_MODULE_15__.MatInputModule,
        _angular_material_card__WEBPACK_IMPORTED_MODULE_16__.MatCardModule, _angular_material_select__WEBPACK_IMPORTED_MODULE_17__.MatSelectModule, _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_18__.MatDatepickerModule, _angular_material_core__WEBPACK_IMPORTED_MODULE_19__.MatNativeDateModule,
        _angular_material_progress_bar__WEBPACK_IMPORTED_MODULE_20__.MatProgressBarModule, _angular_material_table__WEBPACK_IMPORTED_MODULE_21__.MatTableModule, _angular_material_menu__WEBPACK_IMPORTED_MODULE_22__.MatMenuModule, _angular_material_dialog__WEBPACK_IMPORTED_MODULE_23__.MatDialogModule,
        _angular_material_checkbox__WEBPACK_IMPORTED_MODULE_24__.MatCheckboxModule, _angular_material_paginator__WEBPACK_IMPORTED_MODULE_25__.MatPaginatorModule, _angular_material_toolbar__WEBPACK_IMPORTED_MODULE_26__.MatToolbarModule, _angular_material_sidenav__WEBPACK_IMPORTED_MODULE_27__.MatSidenavModule,
        _angular_material_progress_spinner__WEBPACK_IMPORTED_MODULE_28__.MatProgressSpinnerModule], exports: [ngx_bootstrap_dropdown__WEBPACK_IMPORTED_MODULE_2__.BsDropdownModule,
        ngx_toastr__WEBPACK_IMPORTED_MODULE_3__.ToastrModule,
        ngx_bootstrap_tabs__WEBPACK_IMPORTED_MODULE_4__.TabsModule,
        ng2_file_upload__WEBPACK_IMPORTED_MODULE_5__.FileUploadModule,
        ngx_bootstrap_datepicker__WEBPACK_IMPORTED_MODULE_6__.BsDatepickerModule,
        ngx_bootstrap_pagination__WEBPACK_IMPORTED_MODULE_7__.PaginationModule,
        ngx_timeago__WEBPACK_IMPORTED_MODULE_8__.TimeagoModule,
        ngx_bootstrap_buttons__WEBPACK_IMPORTED_MODULE_9__.ButtonsModule,
        _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__.MatTabsModule,
        _angular_material_button__WEBPACK_IMPORTED_MODULE_11__.MatButtonModule,
        _angular_material_form_field__WEBPACK_IMPORTED_MODULE_12__.MatFormFieldModule,
        _angular_material_icon__WEBPACK_IMPORTED_MODULE_13__.MatIconModule,
        _angular_material_grid_list__WEBPACK_IMPORTED_MODULE_14__.MatGridListModule,
        _angular_material_input__WEBPACK_IMPORTED_MODULE_15__.MatInputModule,
        _angular_material_card__WEBPACK_IMPORTED_MODULE_16__.MatCardModule,
        _angular_material_select__WEBPACK_IMPORTED_MODULE_17__.MatSelectModule,
        _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_18__.MatDatepickerModule,
        _angular_material_core__WEBPACK_IMPORTED_MODULE_19__.MatNativeDateModule,
        _angular_material_progress_bar__WEBPACK_IMPORTED_MODULE_20__.MatProgressBarModule,
        _angular_material_table__WEBPACK_IMPORTED_MODULE_21__.MatTableModule,
        _angular_material_menu__WEBPACK_IMPORTED_MODULE_22__.MatMenuModule,
        _angular_material_dialog__WEBPACK_IMPORTED_MODULE_23__.MatDialogModule,
        _angular_material_checkbox__WEBPACK_IMPORTED_MODULE_24__.MatCheckboxModule,
        _angular_material_paginator__WEBPACK_IMPORTED_MODULE_25__.MatPaginatorModule,
        _angular_material_toolbar__WEBPACK_IMPORTED_MODULE_26__.MatToolbarModule,
        _angular_material_sidenav__WEBPACK_IMPORTED_MODULE_27__.MatSidenavModule,
        _angular_material_progress_spinner__WEBPACK_IMPORTED_MODULE_28__.MatProgressSpinnerModule] }); })();


/***/ }),

/***/ 8406:
/*!******************************************************!*\
  !*** ./src/app/_resolvers/user-detailed.resolver.ts ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserDetailedResolver": () => (/* binding */ UserDetailedResolver)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/user.service */ 5089);


class UserDetailedResolver {
    constructor(userService) {
        this.userService = userService;
    }
    resolve(route) {
        return this.userService.getUserByUsername(route.paramMap.get("username"));
    }
}
UserDetailedResolver.ɵfac = function UserDetailedResolver_Factory(t) { return new (t || UserDetailedResolver)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵinject"](_services_user_service__WEBPACK_IMPORTED_MODULE_0__.UserService)); };
UserDetailedResolver.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineInjectable"]({ token: UserDetailedResolver, factory: UserDetailedResolver.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 7110:
/*!**********************************************!*\
  !*** ./src/app/_services/account.service.ts ***!
  \**********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AccountService": () => (/* binding */ AccountService)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ 6067);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ 635);
/* harmony import */ var src_environments_environment__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/environments/environment */ 2340);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/common/http */ 8784);
/* harmony import */ var _presence_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./presence.service */ 3238);





class AccountService {
    constructor(http, presenceService) {
        this.http = http;
        this.presenceService = presenceService;
        this.baseUrl = src_environments_environment__WEBPACK_IMPORTED_MODULE_0__.environment.apiUrl;
        this.currentUserSource = new rxjs__WEBPACK_IMPORTED_MODULE_2__.ReplaySubject(1);
        this.currentUser$ = this.currentUserSource.asObservable();
    }
    login(loginCred) {
        return this.http.post(this.baseUrl + 'account/login', loginCred).pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_3__.map)((response) => {
            const loginResponse = response;
            if (loginResponse) {
                this.setCurrentUser(loginResponse);
                this.presenceService.createHubConnection(response);
            }
            return response;
        }));
    }
    setCurrentUser(loginResponse) {
        loginResponse.userData.roles = [];
        const roles = this.getDecodedToken(loginResponse.token).role;
        Array.isArray(roles) ? loginResponse.userData.roles = roles : loginResponse.userData.roles.push(roles);
        localStorage.setItem('user', JSON.stringify(loginResponse));
        this.currentUserSource.next(loginResponse);
    }
    logout() {
        localStorage.removeItem('user');
        this.presenceService.stopHubConnection();
        this.currentUserSource.next(null);
    }
    register(model) {
        return this.http.post(this.baseUrl + 'account/register', model).pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_3__.map)((response) => {
            if (response) {
                this.setCurrentUser(response);
                this.presenceService.createHubConnection(response);
            }
            return response;
        }));
    }
    getDecodedToken(token) {
        return JSON.parse(atob(token.split('.')[1]));
    }
}
AccountService.ɵfac = function AccountService_Factory(t) { return new (t || AccountService)(_angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_5__.HttpClient), _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵinject"](_presence_service__WEBPACK_IMPORTED_MODULE_1__.PresenceService)); };
AccountService.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdefineInjectable"]({ token: AccountService, factory: AccountService.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 2517:
/*!********************************************!*\
  !*** ./src/app/_services/admin.service.ts ***!
  \********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AdminService": () => (/* binding */ AdminService)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 745);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ 635);
/* harmony import */ var src_environments_environment__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/environments/environment */ 2340);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common/http */ 8784);




class AdminService {
    constructor(http) {
        this.http = http;
        this.baseUrl = src_environments_environment__WEBPACK_IMPORTED_MODULE_0__.environment.apiUrl;
        this.cachedRoles = [];
    }
    getUsersWithRoles() {
        return this.http.get(this.baseUrl + "roles/users-roles/all");
    }
    getAllRoles() {
        if (this.cachedRoles.length > 0) {
            return (0,rxjs__WEBPACK_IMPORTED_MODULE_1__.of)(this.cachedRoles);
        }
        return this.http.get(this.baseUrl + "roles/all").pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_2__.map)(r => {
            this.cachedRoles = r;
            return r;
        }));
    }
    addRoleToUser(username, role) {
        let obj = { 'username': username, 'role': role };
        return this.http.post(this.baseUrl + 'roles/add', obj);
    }
    deleteRoleFromUser(username, role) {
        let obj = { 'username': username, 'role': role };
        return this.http.delete(this.baseUrl + 'roles/removefrom', { body: obj });
    }
}
AdminService.ɵfac = function AdminService_Factory(t) { return new (t || AdminService)(_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_4__.HttpClient)); };
AdminService.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdefineInjectable"]({ token: AdminService, factory: AdminService.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 4109:
/*!*******************************************!*\
  !*** ./src/app/_services/busy.service.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "BusyService": () => (/* binding */ BusyService)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var ngx_spinner__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ngx-spinner */ 3947);


class BusyService {
    constructor(spinnerService) {
        this.spinnerService = spinnerService;
        this.busyRequestCount = 0;
    }
    busy() {
        this.busyRequestCount++;
        this.spinnerService.show();
    }
    idle() {
        this.busyRequestCount--;
        if (this.busyRequestCount < 1) {
            this.busyRequestCount = 0; // for safety just in case
            this.spinnerService.hide();
        }
    }
}
BusyService.ɵfac = function BusyService_Factory(t) { return new (t || BusyService)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵinject"](ngx_spinner__WEBPACK_IMPORTED_MODULE_1__.NgxSpinnerService)); };
BusyService.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjectable"]({ token: BusyService, factory: BusyService.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 6252:
/*!**********************************************!*\
  !*** ./src/app/_services/message.service.ts ***!
  \**********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "MessageService": () => (/* binding */ MessageService)
/* harmony export */ });
/* harmony import */ var E_Programming_projects_Dating_App_WebUI_node_modules_babel_runtime_helpers_esm_asyncToGenerator_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./node_modules/@babel/runtime/helpers/esm/asyncToGenerator.js */ 1670);
/* harmony import */ var _microsoft_signalr__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @microsoft/signalr */ 7930);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! rxjs */ 6317);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var src_environments_environment__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/environments/environment */ 2340);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common/http */ 8784);






class MessageService {
  constructor(http) {
    this.http = http;
    this.baseUrl = src_environments_environment__WEBPACK_IMPORTED_MODULE_1__.environment.apiUrl;
    this.hubUrl = src_environments_environment__WEBPACK_IMPORTED_MODULE_1__.environment.hubUrl;
    this.hubConnection = null;
    this.messageThreadSource = new rxjs__WEBPACK_IMPORTED_MODULE_2__.BehaviorSubject([]);
    this.messageThread$ = this.messageThreadSource.asObservable();
  }

  sendMessage(username, content) {
    var _this = this;

    return (0,E_Programming_projects_Dating_App_WebUI_node_modules_babel_runtime_helpers_esm_asyncToGenerator_js__WEBPACK_IMPORTED_MODULE_0__["default"])(function* () {
      var _a;

      return (_a = _this.hubConnection) === null || _a === void 0 ? void 0 : _a.invoke("SendMessages", {
        "recipientUsername": username,
        "content": content
      }).catch(e => console.log(e));
    })();
  }

  createHubConnection(user, otherUserId) {
    this.hubConnection = new _microsoft_signalr__WEBPACK_IMPORTED_MODULE_3__.HubConnectionBuilder().withUrl(this.hubUrl + 'message?userId=' + otherUserId, {
      accessTokenFactory: () => user.token
    }).withAutomaticReconnect().build();
    this.hubConnection.start().catch(e => console.log(e));
    this.hubConnection.on("ReceiveMessages", r => this.messageThreadSource.next(r));
    this.hubConnection.on("NewMessage", m => {
      console.log(m);
      this.messageThread$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_4__.take)(1)).subscribe(msgs => {
        this.messageThreadSource.next([...msgs, m]);
      });
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  deleteMessage(msgId) {
    return this.http.delete(this.baseUrl + 'Messages/' + msgId);
  }

}

MessageService.ɵfac = function MessageService_Factory(t) {
  return new (t || MessageService)(_angular_core__WEBPACK_IMPORTED_MODULE_5__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_6__.HttpClient));
};

MessageService.ɵprov = /*@__PURE__*/_angular_core__WEBPACK_IMPORTED_MODULE_5__["ɵɵdefineInjectable"]({
  token: MessageService,
  factory: MessageService.ɵfac,
  providedIn: 'root'
});

/***/ }),

/***/ 3238:
/*!***********************************************!*\
  !*** ./src/app/_services/presence.service.ts ***!
  \***********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "PresenceService": () => (/* binding */ PresenceService)
/* harmony export */ });
/* harmony import */ var _microsoft_signalr__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @microsoft/signalr */ 7930);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! rxjs */ 6317);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var src_environments_environment__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/environments/environment */ 2340);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ngx-toastr */ 4101);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/router */ 2816);






class PresenceService {
    constructor(toastr, router) {
        this.toastr = toastr;
        this.router = router;
        this.baseUrl = src_environments_environment__WEBPACK_IMPORTED_MODULE_0__.environment.hubUrl;
        this.onlineUsersSource = new rxjs__WEBPACK_IMPORTED_MODULE_1__.BehaviorSubject([]);
        this.onlineUsers$ = this.onlineUsersSource.asObservable();
        this.hubConnection = null;
    }
    createHubConnection(user) {
        this.hubConnection = new _microsoft_signalr__WEBPACK_IMPORTED_MODULE_2__.HubConnectionBuilder()
            .withUrl(this.baseUrl + 'presence', {
            accessTokenFactory: () => user.token
        })
            .withAutomaticReconnect()
            .build();
        this.hubConnection
            .start()
            .catch(e => { console.log(e); });
        this.hubConnection.on("UserIsOnline", username => {
            this.onlineUsers$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_3__.take)(1))
                .subscribe(usernames => {
                this.onlineUsersSource.next([...usernames, username]);
            });
        });
        this.hubConnection.on("UserIsOffline", username => {
            this.onlineUsers$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_3__.take)(1))
                .subscribe(usernames => {
                this.onlineUsersSource.next([...usernames.filter(u => u != username)]);
            });
        });
        this.hubConnection.on("GetOnlineUsers", (usernames) => {
            this.onlineUsersSource.next(usernames);
        });
        this.hubConnection.on("NewMessage", (userMsg) => {
            console.log(userMsg);
            this.toastr.info(userMsg.senderDTO.firstName + ": " + userMsg.msgDTO.content)
                .onTap
                .pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_3__.take)(1))
                .subscribe(() => {
                this.router.navigateByUrl("/messages", { state: userMsg.senderDTO });
            });
        });
    }
    stopHubConnection() {
        var _a;
        (_a = this.hubConnection) === null || _a === void 0 ? void 0 : _a.stop().catch(e => console.log(e));
    }
}
PresenceService.ɵfac = function PresenceService_Factory(t) { return new (t || PresenceService)(_angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵinject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_5__.ToastrService), _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵinject"](_angular_router__WEBPACK_IMPORTED_MODULE_6__.Router)); };
PresenceService.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdefineInjectable"]({ token: PresenceService, factory: PresenceService.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 5089:
/*!*******************************************!*\
  !*** ./src/app/_services/user.service.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserService": () => (/* binding */ UserService)
/* harmony export */ });
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common/http */ 8784);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! rxjs */ 745);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! rxjs */ 635);
/* harmony import */ var src_environments_environment__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/environments/environment */ 2340);
/* harmony import */ var _models_userParams__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../_models/userParams */ 4057);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _account_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./account.service */ 7110);







class UserService {
    constructor(http, accountService) {
        this.http = http;
        this.accountService = accountService;
        this.baseUrl = src_environments_environment__WEBPACK_IMPORTED_MODULE_0__.environment.apiUrl;
        this.users = []; //used for profile caching
        this.userParams = null; //used for filter caching
        this.paginationInfo = {
            currentPage: 0,
            itemsPerPage: 0,
            totalItems: 0,
            totalPages: 0
        };
        this.userChache = new Map();
        this.resetUserParams();
    }
    getUserParams() {
        return this.userParams;
    }
    setUserParams(params) {
        this.userParams = params;
    }
    resetUserParams() {
        this.accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_3__.take)(1)).subscribe(response => {
            if (response) {
                this.userParams = new _models_userParams__WEBPACK_IMPORTED_MODULE_1__.UserParams(response.userData);
            }
        });
        return this.userParams;
    }
    // helper method
    getPaginationParams(userParams) {
        let httpParams = new _angular_common_http__WEBPACK_IMPORTED_MODULE_4__.HttpParams()
            .set('pageNumber', userParams.pageNumber)
            .set('itemsPerPage', userParams.itemsPerPage)
            .set('sex', userParams.sex)
            .set('orderBy', userParams.orderBy);
        if (userParams.minAge) {
            httpParams = httpParams.set('minAge', userParams.minAge);
        }
        if (userParams.maxAge) {
            httpParams = httpParams.set('maxAge', userParams.maxAge);
        }
        return httpParams;
    }
    deleteCachedValues() {
        this.userChache = new Map();
        this.resetUserParams();
    }
    getAllUsers(userParams) {
        let paginatedResult = { result: [], pagination: this.paginationInfo };
        let cache = this.userChache.get(Object.values(userParams).join('-'));
        if (cache) {
            paginatedResult.result = cache;
            return (0,rxjs__WEBPACK_IMPORTED_MODULE_5__.of)(paginatedResult);
        }
        return this.http.get(this.baseUrl + 'users/all', { observe: 'response', params: this.getPaginationParams(userParams) })
            .pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_6__.map)(response => {
            if (response === null || response === void 0 ? void 0 : response.body) {
                paginatedResult.result = response.body;
            }
            if (response.headers.get('Pagination')) {
                paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
            }
            this.userChache.set(Object.values(userParams).join('-'), paginatedResult.result);
            this.paginationInfo = paginatedResult.pagination;
            return paginatedResult;
        }));
    }
    getUserByUsername(username) {
        const user = this.users.find(u => u.username === username);
        if (user) {
            return (0,rxjs__WEBPACK_IMPORTED_MODULE_5__.of)(user);
        }
        return this.http.get(this.baseUrl + 'users/info/username/' + username);
    }
    reorderPhotos(photos) {
        return this.http.put(this.baseUrl + 'users/photos/reorder', photos);
    }
    deletePhoto(photoId) {
        return this.http.delete(this.baseUrl + 'users/photo/delete/' + String(photoId));
    }
    like(username) {
        return this.http.post(this.baseUrl + 'like/' + username, {}).pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_6__.map)(r => {
            //the correct answer will need a more complex caching system so we will only delete all the cashed data for now
            this.userChache = new Map();
            return r;
        }));
    }
    getLikes() {
        return this.http.get(this.baseUrl + 'like/liked');
    }
    getIsLiked(username) {
        return this.http.get(this.baseUrl + 'like/isliked/' + username);
    }
    getIsMatch(username) {
        return this.http.get(this.baseUrl + 'matches/ismatch/' + username);
    }
    getMatches(pageNumber = 1, itemsPerPage = 2) {
        let paginatedResult = { result: [], pagination: this.paginationInfo };
        let httpParams = new _angular_common_http__WEBPACK_IMPORTED_MODULE_4__.HttpParams()
            .set('pageNumber', pageNumber)
            .set('itemsPerPage', itemsPerPage);
        let cache = this.userChache.get(pageNumber + '-' + itemsPerPage);
        if (cache) {
            paginatedResult.result = cache;
            return (0,rxjs__WEBPACK_IMPORTED_MODULE_5__.of)(paginatedResult);
        }
        return this.http.get(this.baseUrl + 'matches', { observe: 'response', params: httpParams })
            .pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_6__.map)(response => {
            if (response === null || response === void 0 ? void 0 : response.body) {
                paginatedResult.result = response.body;
            }
            if (response.headers.get('Pagination')) {
                paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
            }
            this.userChache.set(pageNumber + '-' + itemsPerPage, paginatedResult.result);
            this.paginationInfo = paginatedResult.pagination;
            return paginatedResult;
        }));
    }
    updateUser(user) {
        const userTosend = {
            firstName: user.firstName,
            lastName: user.lastName,
            interest: user.interest,
            bio: user.bio,
            city: user.city,
            country: user.country
        };
        return this.http.put(this.baseUrl + 'users/update', userTosend).pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_6__.map)(() => {
            const index = this.users.indexOf(user);
            this.users[index] = user;
            return user;
        }));
    }
    getPhotos() {
        return this.http.get(this.baseUrl + "users/photos/all");
    }
}
UserService.ɵfac = function UserService_Factory(t) { return new (t || UserService)(_angular_core__WEBPACK_IMPORTED_MODULE_7__["ɵɵinject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_4__.HttpClient), _angular_core__WEBPACK_IMPORTED_MODULE_7__["ɵɵinject"](_account_service__WEBPACK_IMPORTED_MODULE_2__.AccountService)); };
UserService.ɵprov = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_7__["ɵɵdefineInjectable"]({ token: UserService, factory: UserService.ɵfac, providedIn: 'root' });


/***/ }),

/***/ 4752:
/*!************************************************************!*\
  !*** ./src/app/admin/admin-panel/admin-panel.component.ts ***!
  \************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AdminPanelComponent": () => (/* binding */ AdminPanelComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_material_tabs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/material/tabs */ 2379);
/* harmony import */ var _directives_has_role_directive__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../_directives/has-role.directive */ 397);
/* harmony import */ var _user_management_user_management_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../user-management/user-management.component */ 8975);
/* harmony import */ var _photo_management_photo_management_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../photo-management/photo-management.component */ 5036);





function AdminPanelComponent_mat_tab_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "mat-tab", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](1, "app-user-management");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} }
const _c0 = function () { return ["admin"]; };
class AdminPanelComponent {
    constructor() { }
    ngOnInit() {
    }
}
AdminPanelComponent.ɵfac = function AdminPanelComponent_Factory(t) { return new (t || AdminPanelComponent)(); };
AdminPanelComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdefineComponent"]({ type: AdminPanelComponent, selectors: [["app-admin-panel"]], decls: 6, vars: 2, consts: [["mat-align-tabs", "start"], ["label", "User Management", 4, "appHasRole"], ["label", "Photo Management"], ["label", "User Management"]], template: function AdminPanelComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "h1");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](1, "Management Panel");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](2, "mat-tab-group", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](3, AdminPanelComponent_mat_tab_3_Template, 2, 0, "mat-tab", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](4, "mat-tab", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](5, "app-photo-management");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("appHasRole", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpureFunction0"](1, _c0));
    } }, directives: [_angular_material_tabs__WEBPACK_IMPORTED_MODULE_4__.MatTabGroup, _directives_has_role_directive__WEBPACK_IMPORTED_MODULE_0__.HasRoleDirective, _angular_material_tabs__WEBPACK_IMPORTED_MODULE_4__.MatTab, _user_management_user_management_component__WEBPACK_IMPORTED_MODULE_1__.UserManagementComponent, _photo_management_photo_management_component__WEBPACK_IMPORTED_MODULE_2__.PhotoManagementComponent], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJhZG1pbi1wYW5lbC5jb21wb25lbnQuY3NzIn0= */"] });


/***/ }),

/***/ 5036:
/*!**********************************************************************!*\
  !*** ./src/app/admin/photo-management/photo-management.component.ts ***!
  \**********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "PhotoManagementComponent": () => (/* binding */ PhotoManagementComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);

class PhotoManagementComponent {
    constructor() { }
    ngOnInit() {
    }
}
PhotoManagementComponent.ɵfac = function PhotoManagementComponent_Factory(t) { return new (t || PhotoManagementComponent)(); };
PhotoManagementComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: PhotoManagementComponent, selectors: [["app-photo-management"]], decls: 2, vars: 0, template: function PhotoManagementComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "p");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1, "photo-management not implemented yet!");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } }, styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJwaG90by1tYW5hZ2VtZW50LmNvbXBvbmVudC5jc3MifQ== */"] });


/***/ }),

/***/ 4351:
/*!**********************************************************************!*\
  !*** ./src/app/admin/role-edit-dialog/role-edit-dialog.component.ts ***!
  \**********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "RoleEditDialogComponent": () => (/* binding */ RoleEditDialogComponent)
/* harmony export */ });
/* harmony import */ var _angular_material_dialog__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/material/dialog */ 5758);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var src_app_services_admin_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/app/_services/admin.service */ 2517);
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/forms */ 587);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_checkbox__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/material/checkbox */ 1534);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/material/button */ 7317);








function RoleEditDialogComponent_mat_checkbox_7_Template(rf, ctx) { if (rf & 1) {
    const _r4 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "mat-checkbox", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("ngModelChange", function RoleEditDialogComponent_mat_checkbox_7_Template_mat_checkbox_ngModelChange_0_listener($event) { const restoredCtx = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵrestoreView"](_r4); const role_r2 = restoredCtx.$implicit; const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"](); return (ctx_r3.rolesForm[role_r2] = $event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
} if (rf & 2) {
    const role_r2 = ctx.$implicit;
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("name", role_r2)("ngModel", ctx_r1.rolesForm[role_r2]);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](role_r2);
} }
class RoleEditDialogComponent {
    constructor(dialogRef, data, adminSerive) {
        this.dialogRef = dialogRef;
        this.data = data;
        this.adminSerive = adminSerive;
        this.roles = [];
        this.rolesToAdd = [];
        this.rolesToDelete = [];
        this.rolesForm = {};
        adminSerive.getAllRoles().subscribe(r => this.roles = r);
        for (let role of this.roles) {
            this.rolesForm[role] = this.data.roles.includes(role);
        }
    }
    ngOnInit() {
    }
    onNoClick() {
        this.dialogRef.close();
    }
    isRolesChanged() {
        let role;
        for (role in this.rolesForm) {
            if (this.rolesForm[role] === false) {
                if (this.data.roles.includes(role)) {
                    return true;
                }
            }
            else if (this.rolesForm[role] === true) {
                if (!this.data.roles.includes(role)) {
                    return true;
                }
            }
        }
        return false;
    }
    whichRoleChanged() {
        let role;
        for (role in this.rolesForm) {
            if (this.rolesForm[role] === false) {
                if (this.data.roles.includes(role)) {
                    this.rolesToDelete.push(role);
                }
            }
            else if (this.rolesForm[role] === true) {
                if (!this.data.roles.includes(role)) {
                    this.rolesToAdd.push(role);
                }
            }
        }
        return false;
    }
    updateRoles() {
        this.whichRoleChanged();
        for (let role of this.rolesToAdd) {
            this.adminSerive.addRoleToUser(this.data.username, role).subscribe(r => {
                this.data.roles.push(role);
            });
        }
        for (let role of this.rolesToDelete) {
            this.adminSerive.deleteRoleFromUser(this.data.username, role).subscribe((r) => {
                this.data.roles = this.data.roles.filter(r => r != role);
            });
        }
    }
}
RoleEditDialogComponent.ɵfac = function RoleEditDialogComponent_Factory(t) { return new (t || RoleEditDialogComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_material_dialog__WEBPACK_IMPORTED_MODULE_2__.MatDialogRef), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](_angular_material_dialog__WEBPACK_IMPORTED_MODULE_2__.MAT_DIALOG_DATA), _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdirectiveInject"](src_app_services_admin_service__WEBPACK_IMPORTED_MODULE_0__.AdminService)); };
RoleEditDialogComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵdefineComponent"]({ type: RoleEditDialogComponent, selectors: [["app-role-edit-dialog"]], decls: 13, vars: 3, consts: [["mat-dialog-title", ""], [1, "mat-typography"], [1, "row"], ["ngForm", "ngForm"], [3, "name", "ngModel", "ngModelChange", 4, "ngFor", "ngForOf"], ["align", "end"], ["mat-button", "", 3, "click"], ["mat-button", "", 3, "mat-dialog-close", "click"], [3, "name", "ngModel", "ngModelChange"]], template: function RoleEditDialogComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](0, "h2", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](2, "mat-dialog-content", 1)(3, "form", 2, 3)(5, "h3");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](6, "Roles");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtemplate"](7, RoleEditDialogComponent_mat_checkbox_7_Template, 2, 3, "mat-checkbox", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](8, "mat-dialog-actions", 5)(9, "button", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function RoleEditDialogComponent_Template_button_click_9_listener() { return ctx.onNoClick(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](10, "Dismiss");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementStart"](11, "button", 7);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵlistener"]("click", function RoleEditDialogComponent_Template_button_click_11_listener() { return ctx.updateRoles(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtext"](12, "Save");
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵelementEnd"]()();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵtextInterpolate"](ctx.data.username);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](6);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("ngForOf", ctx.roles);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_1__["ɵɵproperty"]("mat-dialog-close", ctx.data.roles);
    } }, directives: [_angular_material_dialog__WEBPACK_IMPORTED_MODULE_2__.MatDialogTitle, _angular_material_dialog__WEBPACK_IMPORTED_MODULE_2__.MatDialogContent, _angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵNgNoValidate"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__.NgControlStatusGroup, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.NgForm, _angular_common__WEBPACK_IMPORTED_MODULE_4__.NgForOf, _angular_material_checkbox__WEBPACK_IMPORTED_MODULE_5__.MatCheckbox, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.NgControlStatus, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.NgModel, _angular_material_dialog__WEBPACK_IMPORTED_MODULE_2__.MatDialogActions, _angular_material_button__WEBPACK_IMPORTED_MODULE_6__.MatButton, _angular_material_dialog__WEBPACK_IMPORTED_MODULE_2__.MatDialogClose], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJyb2xlLWVkaXQtZGlhbG9nLmNvbXBvbmVudC5jc3MifQ== */"] });


/***/ }),

/***/ 8975:
/*!********************************************************************!*\
  !*** ./src/app/admin/user-management/user-management.component.ts ***!
  \********************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserManagementComponent": () => (/* binding */ UserManagementComponent)
/* harmony export */ });
/* harmony import */ var _role_edit_dialog_role_edit_dialog_component__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../role-edit-dialog/role-edit-dialog.component */ 4351);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var src_app_services_admin_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/_services/admin.service */ 2517);
/* harmony import */ var _angular_material_dialog__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/material/dialog */ 5758);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_table__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/material/table */ 7217);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_material_icon__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/icon */ 5590);








function UserManagementComponent_table_1_th_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "th", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Id ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} }
function UserManagementComponent_table_1_td_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "td", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r11 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](item_r11.id);
} }
function UserManagementComponent_table_1_th_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "th", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Username ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} }
function UserManagementComponent_table_1_td_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "td", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r12 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](item_r12.username);
} }
function UserManagementComponent_table_1_th_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "th", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " Roles ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} }
function UserManagementComponent_table_1_td_9_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "td", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r13 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](item_r13.roles.join("/"));
} }
function UserManagementComponent_table_1_th_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](0, "th", 11);
} }
function UserManagementComponent_table_1_td_12_Template(rf, ctx) { if (rf & 1) {
    const _r16 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "td", 12)(1, "button", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("click", function UserManagementComponent_table_1_td_12_Template_button_click_1_listener() { const restoredCtx = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r16); const item_r14 = restoredCtx.$implicit; const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2); return ctx_r15.openDialog(item_r14); });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](2, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](3, "edit");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](4, " Edit Roles ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
} }
function UserManagementComponent_table_1_tr_13_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](0, "tr", 14);
} }
function UserManagementComponent_table_1_tr_14_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](0, "tr", 15);
} }
const _c0 = function () { return ["id", "username", "roles", "edit-buttons"]; };
function UserManagementComponent_table_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "table", 2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerStart"](1, 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](2, UserManagementComponent_table_1_th_2_Template, 2, 0, "th", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](3, UserManagementComponent_table_1_td_3_Template, 2, 1, "td", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerStart"](4, 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](5, UserManagementComponent_table_1_th_5_Template, 2, 0, "th", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](6, UserManagementComponent_table_1_td_6_Template, 2, 1, "td", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerStart"](7, 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](8, UserManagementComponent_table_1_th_8_Template, 2, 0, "th", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](9, UserManagementComponent_table_1_td_9_Template, 2, 1, "td", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerStart"](10, 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](11, UserManagementComponent_table_1_th_11_Template, 1, 0, "th", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](12, UserManagementComponent_table_1_td_12_Template, 5, 0, "td", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](13, UserManagementComponent_table_1_tr_13_Template, 1, 0, "tr", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](14, UserManagementComponent_table_1_tr_14_Template, 1, 0, "tr", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("dataSource", ctx_r0.usersRoles);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](13);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("matHeaderRowDef", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpureFunction0"](3, _c0));
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("matRowDefColumns", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpureFunction0"](4, _c0));
} }
class UserManagementComponent {
    constructor(adminService, dialog) {
        this.dialog = dialog;
        this.usersRoles = [];
        this.allRoles = [];
        adminService.getUsersWithRoles().subscribe(r => {
            if (r) {
                this.usersRoles = r;
            }
        });
        adminService.getAllRoles().subscribe(r => {
            if (r) {
                this.allRoles = r;
            }
        });
    }
    openDialog(roleUser) {
        const dialogRef = this.dialog.open(_role_edit_dialog_role_edit_dialog_component__WEBPACK_IMPORTED_MODULE_0__.RoleEditDialogComponent, { data: roleUser });
        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                roleUser.roles = result;
            }
        });
    }
    ngOnInit() {
    }
}
UserManagementComponent.ɵfac = function UserManagementComponent_Factory(t) { return new (t || UserManagementComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](src_app_services_admin_service__WEBPACK_IMPORTED_MODULE_1__.AdminService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_material_dialog__WEBPACK_IMPORTED_MODULE_3__.MatDialog)); };
UserManagementComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({ type: UserManagementComponent, selectors: [["app-user-management"]], decls: 2, vars: 1, consts: [[1, "row", "mx-0"], ["mat-table", "", "class", "mat-elevation-z8", 3, "dataSource", 4, "ngIf"], ["mat-table", "", 1, "mat-elevation-z8", 3, "dataSource"], ["matColumnDef", "id", 2, "width", "10px"], ["mat-header-cell", "", 4, "matHeaderCellDef"], ["mat-cell", "", 4, "matCellDef"], ["matColumnDef", "username"], ["matColumnDef", "roles"], ["matColumnDef", "edit-buttons"], ["mat-header-row", "", 4, "matHeaderRowDef"], ["mat-row", "", 4, "matRowDef", "matRowDefColumns"], ["mat-header-cell", ""], ["mat-cell", ""], ["mat-flat-button", "", "color", "primary", 3, "click"], ["mat-header-row", ""], ["mat-row", ""]], template: function UserManagementComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, UserManagementComponent_table_1_Template, 15, 5, "table", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.usersRoles.length > 0);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_4__.NgIf, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatTable, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatColumnDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatHeaderCellDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatHeaderCell, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatCellDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatCell, _angular_material_button__WEBPACK_IMPORTED_MODULE_6__.MatButton, _angular_material_icon__WEBPACK_IMPORTED_MODULE_7__.MatIcon, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatHeaderRowDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatHeaderRow, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatRowDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_5__.MatRow], styles: [".mat-column-edit-buttons[_ngcontent-%COMP%] {\r\n    width: 10%;\r\n    text-align: center;\r\n    font-weight: bold;\r\n  }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInVzZXItbWFuYWdlbWVudC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksVUFBVTtJQUNWLGtCQUFrQjtJQUNsQixpQkFBaUI7RUFDbkIiLCJmaWxlIjoidXNlci1tYW5hZ2VtZW50LmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIubWF0LWNvbHVtbi1lZGl0LWJ1dHRvbnMge1xyXG4gICAgd2lkdGg6IDEwJTtcclxuICAgIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICAgIGZvbnQtd2VpZ2h0OiBib2xkO1xyXG4gIH0iXX0= */"] });


/***/ }),

/***/ 5041:
/*!**********************************!*\
  !*** ./src/app/app.component.ts ***!
  \**********************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AppComponent": () => (/* binding */ AppComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./_services/account.service */ 7110);
/* harmony import */ var _services_presence_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./_services/presence.service */ 3238);
/* harmony import */ var ngx_spinner__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-spinner */ 3947);
/* harmony import */ var _nav_nav_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./nav/nav.component */ 5375);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/router */ 2816);






class AppComponent {
    //public users?: User[];
    constructor(accountService, presenceService) {
        this.accountService = accountService;
        this.presenceService = presenceService;
        this.title = 'Dating App';
    }
    ngOnInit() {
        this.setCurrentUser();
    }
    setCurrentUser() {
        let userString = localStorage.getItem('user');
        if (userString != null) {
            let user = JSON.parse(userString);
            this.accountService.setCurrentUser(user);
            this.presenceService.createHubConnection(user);
        }
    }
}
AppComponent.ɵfac = function AppComponent_Factory(t) { return new (t || AppComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](_services_account_service__WEBPACK_IMPORTED_MODULE_0__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](_services_presence_service__WEBPACK_IMPORTED_MODULE_1__.PresenceService)); };
AppComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdefineComponent"]({ type: AppComponent, selectors: [["app-root"]], decls: 6, vars: 1, consts: [["bdColor", "rgba(0, 0, 0, 0.8)", "size", "medium", "color", "#fff", "type", "ball-spin", 3, "fullScreen"], [2, "color", "white"], [1, "mt-5", "container"]], template: function AppComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "ngx-spinner", 0)(1, "p", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](2, " Loading... ");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](3, "app-nav");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](4, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](5, "router-outlet");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("fullScreen", true);
    } }, directives: [ngx_spinner__WEBPACK_IMPORTED_MODULE_4__.NgxSpinnerComponent, _nav_nav_component__WEBPACK_IMPORTED_MODULE_2__.NavComponent, _angular_router__WEBPACK_IMPORTED_MODULE_5__.RouterOutlet], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJhcHAuY29tcG9uZW50LmNzcyJ9 */"] });


/***/ }),

/***/ 6747:
/*!*******************************!*\
  !*** ./src/app/app.module.ts ***!
  \*******************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "AppModule": () => (/* binding */ AppModule)
/* harmony export */ });
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_30__ = __webpack_require__(/*! @angular/common/http */ 8784);
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_31__ = __webpack_require__(/*! @angular/platform-browser */ 318);
/* harmony import */ var _angular_cdk_drag_drop__WEBPACK_IMPORTED_MODULE_37__ = __webpack_require__(/*! @angular/cdk/drag-drop */ 4941);
/* harmony import */ var _app_component__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./app.component */ 5041);
/* harmony import */ var _angular_platform_browser_animations__WEBPACK_IMPORTED_MODULE_32__ = __webpack_require__(/*! @angular/platform-browser/animations */ 3598);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_34__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_33__ = __webpack_require__(/*! @angular/forms */ 587);
/* harmony import */ var _nav_nav_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./nav/nav.component */ 5375);
/* harmony import */ var _footer_footer_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./footer/footer.component */ 970);
/* harmony import */ var _home_home_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./home/home.component */ 5067);
/* harmony import */ var _register_register_component__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./register/register.component */ 9087);
/* harmony import */ var _users_user_list_user_list_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./users/user-list/user-list.component */ 1771);
/* harmony import */ var _users_user_detail_user_detail_component__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ./users/user-detail/user-detail.component */ 2545);
/* harmony import */ var _lists_lists_component__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./lists/lists.component */ 6558);
/* harmony import */ var _messages_messages_component__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! ./messages/messages.component */ 5298);
/* harmony import */ var _guards_auth_guard__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ./_guards/auth.guard */ 1094);
/* harmony import */ var _modules_shared_module__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! ./_modules/shared.module */ 9821);
/* harmony import */ var _login_login_component__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ./login/login.component */ 8458);
/* harmony import */ var _errors_test_error_test_error_component__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! ./errors/test-error/test-error.component */ 6175);
/* harmony import */ var _interceptors_error_interceptor__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ./_interceptors/error.interceptor */ 8971);
/* harmony import */ var _errors_not_found_not_found_component__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! ./errors/not-found/not-found.component */ 9953);
/* harmony import */ var _errors_server_error_server_error_component__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(/*! ./errors/server-error/server-error.component */ 2);
/* harmony import */ var _users_user_card_user_card_component__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(/*! ./users/user-card/user-card.component */ 9412);
/* harmony import */ var _interceptors_jwt_interceptor__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(/*! ./_interceptors/jwt.interceptor */ 9707);
/* harmony import */ var _kolkov_ngx_gallery__WEBPACK_IMPORTED_MODULE_35__ = __webpack_require__(/*! @kolkov/ngx-gallery */ 9222);
/* harmony import */ var _users_user_edit_user_edit_component__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(/*! ./users/user-edit/user-edit.component */ 1923);
/* harmony import */ var _guards_prevent_unsaved_changes_guard__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(/*! ./_guards/prevent-unsaved-changes.guard */ 8225);
/* harmony import */ var ngx_spinner__WEBPACK_IMPORTED_MODULE_36__ = __webpack_require__(/*! ngx-spinner */ 3947);
/* harmony import */ var _interceptors_loading_interceptor__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(/*! ./_interceptors/loading.interceptor */ 2046);
/* harmony import */ var _users_photo_editor_photo_editor_component__WEBPACK_IMPORTED_MODULE_21__ = __webpack_require__(/*! ./users/photo-editor/photo-editor.component */ 6883);
/* harmony import */ var _fortawesome_angular_fontawesome__WEBPACK_IMPORTED_MODULE_38__ = __webpack_require__(/*! @fortawesome/angular-fontawesome */ 9397);
/* harmony import */ var _resolvers_user_detailed_resolver__WEBPACK_IMPORTED_MODULE_22__ = __webpack_require__(/*! ./_resolvers/user-detailed.resolver */ 8406);
/* harmony import */ var _admin_admin_panel_admin_panel_component__WEBPACK_IMPORTED_MODULE_23__ = __webpack_require__(/*! ./admin/admin-panel/admin-panel.component */ 4752);
/* harmony import */ var _guards_admin_guard__WEBPACK_IMPORTED_MODULE_24__ = __webpack_require__(/*! ./_guards/admin.guard */ 6299);
/* harmony import */ var _directives_has_role_directive__WEBPACK_IMPORTED_MODULE_25__ = __webpack_require__(/*! ./_directives/has-role.directive */ 397);
/* harmony import */ var _admin_user_management_user_management_component__WEBPACK_IMPORTED_MODULE_26__ = __webpack_require__(/*! ./admin/user-management/user-management.component */ 8975);
/* harmony import */ var _admin_photo_management_photo_management_component__WEBPACK_IMPORTED_MODULE_27__ = __webpack_require__(/*! ./admin/photo-management/photo-management.component */ 5036);
/* harmony import */ var _admin_role_edit_dialog_role_edit_dialog_component__WEBPACK_IMPORTED_MODULE_28__ = __webpack_require__(/*! ./admin/role-edit-dialog/role-edit-dialog.component */ 4351);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_29__ = __webpack_require__(/*! @angular/core */ 3184);








































const routes = [
    { path: 'home', component: _home_home_component__WEBPACK_IMPORTED_MODULE_3__.HomeComponent },
    { path: 'login', component: _login_login_component__WEBPACK_IMPORTED_MODULE_11__.LoginComponent },
    { path: 'register', component: _register_register_component__WEBPACK_IMPORTED_MODULE_4__.RegisterComponent },
    { path: 'users', component: _users_user_list_user_list_component__WEBPACK_IMPORTED_MODULE_5__.UserListComponent, canActivate: [_guards_auth_guard__WEBPACK_IMPORTED_MODULE_9__.AuthGuard] },
    { path: 'users/username/:username', component: _users_user_detail_user_detail_component__WEBPACK_IMPORTED_MODULE_6__.UserDetailComponent, canActivate: [_guards_auth_guard__WEBPACK_IMPORTED_MODULE_9__.AuthGuard], resolve: { user: _resolvers_user_detailed_resolver__WEBPACK_IMPORTED_MODULE_22__.UserDetailedResolver } },
    { path: 'lists', component: _lists_lists_component__WEBPACK_IMPORTED_MODULE_7__.ListsComponent, canActivate: [_guards_auth_guard__WEBPACK_IMPORTED_MODULE_9__.AuthGuard] },
    { path: 'messages', component: _messages_messages_component__WEBPACK_IMPORTED_MODULE_8__.MessagesComponent, canActivate: [_guards_auth_guard__WEBPACK_IMPORTED_MODULE_9__.AuthGuard] },
    { path: 'admin', component: _admin_admin_panel_admin_panel_component__WEBPACK_IMPORTED_MODULE_23__.AdminPanelComponent, canActivate: [_guards_auth_guard__WEBPACK_IMPORTED_MODULE_9__.AuthGuard, _guards_admin_guard__WEBPACK_IMPORTED_MODULE_24__.AdminGuard] },
    { path: 'users/edit', component: _users_user_edit_user_edit_component__WEBPACK_IMPORTED_MODULE_18__.UserEditComponent, canActivate: [_guards_auth_guard__WEBPACK_IMPORTED_MODULE_9__.AuthGuard], canDeactivate: [_guards_prevent_unsaved_changes_guard__WEBPACK_IMPORTED_MODULE_19__.PreventUnsavedChangesGuard] },
    /*{
      path: '',
      runGuardsAndResolvers: 'always',
      canActivate: [AuthGuard],
      children: [
        { path: 'users', component: UserListComponent },
        { path: 'users/username/:username', component: UserDetailComponent },
        { path: 'lists', component: ListsComponent },
        { path: 'messages', component: MessagesComponent }
      ]
    },*/
    { path: 'errors', component: _errors_test_error_test_error_component__WEBPACK_IMPORTED_MODULE_12__.TestErrorComponent },
    { path: 'not-found', component: _errors_not_found_not_found_component__WEBPACK_IMPORTED_MODULE_14__.NotFoundComponent },
    { path: 'server-error', component: _errors_server_error_server_error_component__WEBPACK_IMPORTED_MODULE_15__.ServerErrorComponent },
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: '**', redirectTo: '/not-found', pathMatch: 'full' }
];
class AppModule {
}
AppModule.ɵfac = function AppModule_Factory(t) { return new (t || AppModule)(); };
AppModule.ɵmod = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_29__["ɵɵdefineNgModule"]({ type: AppModule, bootstrap: [_app_component__WEBPACK_IMPORTED_MODULE_0__.AppComponent] });
AppModule.ɵinj = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_29__["ɵɵdefineInjector"]({ providers: [
        { provide: _angular_common_http__WEBPACK_IMPORTED_MODULE_30__.HTTP_INTERCEPTORS, useClass: _interceptors_error_interceptor__WEBPACK_IMPORTED_MODULE_13__.ErrorInterceptor, multi: true },
        { provide: _angular_common_http__WEBPACK_IMPORTED_MODULE_30__.HTTP_INTERCEPTORS, useClass: _interceptors_jwt_interceptor__WEBPACK_IMPORTED_MODULE_17__.JwtInterceptor, multi: true },
        { provide: _angular_common_http__WEBPACK_IMPORTED_MODULE_30__.HTTP_INTERCEPTORS, useClass: _interceptors_loading_interceptor__WEBPACK_IMPORTED_MODULE_20__.LoadingInterceptor, multi: true }
    ], imports: [[
            _angular_platform_browser__WEBPACK_IMPORTED_MODULE_31__.BrowserModule, _angular_common_http__WEBPACK_IMPORTED_MODULE_30__.HttpClientModule, _angular_platform_browser_animations__WEBPACK_IMPORTED_MODULE_32__.BrowserAnimationsModule, _angular_forms__WEBPACK_IMPORTED_MODULE_33__.FormsModule, _angular_forms__WEBPACK_IMPORTED_MODULE_33__.ReactiveFormsModule,
            _angular_router__WEBPACK_IMPORTED_MODULE_34__.RouterModule.forRoot(routes),
            _modules_shared_module__WEBPACK_IMPORTED_MODULE_10__.SharedModule, _angular_common_http__WEBPACK_IMPORTED_MODULE_30__.HttpClientModule, _kolkov_ngx_gallery__WEBPACK_IMPORTED_MODULE_35__.NgxGalleryModule, ngx_spinner__WEBPACK_IMPORTED_MODULE_36__.NgxSpinnerModule, _angular_cdk_drag_drop__WEBPACK_IMPORTED_MODULE_37__.DragDropModule, _fortawesome_angular_fontawesome__WEBPACK_IMPORTED_MODULE_38__.FontAwesomeModule
        ]] });
(function () { (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_29__["ɵɵsetNgModuleScope"](AppModule, { declarations: [_app_component__WEBPACK_IMPORTED_MODULE_0__.AppComponent,
        _nav_nav_component__WEBPACK_IMPORTED_MODULE_1__.NavComponent,
        _footer_footer_component__WEBPACK_IMPORTED_MODULE_2__.FooterComponent,
        _home_home_component__WEBPACK_IMPORTED_MODULE_3__.HomeComponent,
        _register_register_component__WEBPACK_IMPORTED_MODULE_4__.RegisterComponent,
        _users_user_list_user_list_component__WEBPACK_IMPORTED_MODULE_5__.UserListComponent,
        _users_user_detail_user_detail_component__WEBPACK_IMPORTED_MODULE_6__.UserDetailComponent,
        _lists_lists_component__WEBPACK_IMPORTED_MODULE_7__.ListsComponent,
        _messages_messages_component__WEBPACK_IMPORTED_MODULE_8__.MessagesComponent,
        _login_login_component__WEBPACK_IMPORTED_MODULE_11__.LoginComponent,
        _errors_test_error_test_error_component__WEBPACK_IMPORTED_MODULE_12__.TestErrorComponent,
        _errors_not_found_not_found_component__WEBPACK_IMPORTED_MODULE_14__.NotFoundComponent,
        _errors_server_error_server_error_component__WEBPACK_IMPORTED_MODULE_15__.ServerErrorComponent,
        _users_user_card_user_card_component__WEBPACK_IMPORTED_MODULE_16__.UserCardComponent,
        _users_user_edit_user_edit_component__WEBPACK_IMPORTED_MODULE_18__.UserEditComponent,
        _users_photo_editor_photo_editor_component__WEBPACK_IMPORTED_MODULE_21__.PhotoEditorComponent,
        _admin_admin_panel_admin_panel_component__WEBPACK_IMPORTED_MODULE_23__.AdminPanelComponent,
        _directives_has_role_directive__WEBPACK_IMPORTED_MODULE_25__.HasRoleDirective,
        _admin_user_management_user_management_component__WEBPACK_IMPORTED_MODULE_26__.UserManagementComponent,
        _admin_photo_management_photo_management_component__WEBPACK_IMPORTED_MODULE_27__.PhotoManagementComponent,
        _admin_role_edit_dialog_role_edit_dialog_component__WEBPACK_IMPORTED_MODULE_28__.RoleEditDialogComponent], imports: [_angular_platform_browser__WEBPACK_IMPORTED_MODULE_31__.BrowserModule, _angular_common_http__WEBPACK_IMPORTED_MODULE_30__.HttpClientModule, _angular_platform_browser_animations__WEBPACK_IMPORTED_MODULE_32__.BrowserAnimationsModule, _angular_forms__WEBPACK_IMPORTED_MODULE_33__.FormsModule, _angular_forms__WEBPACK_IMPORTED_MODULE_33__.ReactiveFormsModule, _angular_router__WEBPACK_IMPORTED_MODULE_34__.RouterModule, _modules_shared_module__WEBPACK_IMPORTED_MODULE_10__.SharedModule, _angular_common_http__WEBPACK_IMPORTED_MODULE_30__.HttpClientModule, _kolkov_ngx_gallery__WEBPACK_IMPORTED_MODULE_35__.NgxGalleryModule, ngx_spinner__WEBPACK_IMPORTED_MODULE_36__.NgxSpinnerModule, _angular_cdk_drag_drop__WEBPACK_IMPORTED_MODULE_37__.DragDropModule, _fortawesome_angular_fontawesome__WEBPACK_IMPORTED_MODULE_38__.FontAwesomeModule] }); })();


/***/ }),

/***/ 9953:
/*!*********************************************************!*\
  !*** ./src/app/errors/not-found/not-found.component.ts ***!
  \*********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "NotFoundComponent": () => (/* binding */ NotFoundComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ 2816);


class NotFoundComponent {
    constructor() { }
    ngOnInit() {
    }
}
NotFoundComponent.ɵfac = function NotFoundComponent_Factory(t) { return new (t || NotFoundComponent)(); };
NotFoundComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: NotFoundComponent, selectors: [["app-not-found"]], decls: 9, vars: 0, consts: [[1, "row", "justify-content-center"], [1, "mt-5", "col-6"], ["src", "../../../assets/SVGs/NotFound.svg", "alt", "404"], [1, "row", "justify-content-center", "mt-5"], [1, "text-center", "mt-2"], ["routerLink", "/home"]], template: function NotFoundComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0)(1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](2, "img", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "h2", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Oops!");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, " Sorry, the page you\u2019re looking for doesn\u2019t exist. Go back to ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "a", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Home");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()()()();
    } }, directives: [_angular_router__WEBPACK_IMPORTED_MODULE_1__.RouterLinkWithHref], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJub3QtZm91bmQuY29tcG9uZW50LmNzcyJ9 */"] });


/***/ }),

/***/ 2:
/*!***************************************************************!*\
  !*** ./src/app/errors/server-error/server-error.component.ts ***!
  \***************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "ServerErrorComponent": () => (/* binding */ ServerErrorComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ 6362);



function ServerErrorComponent_ng_container_104_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "h5", 99);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "code", 100);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" Error: ", ctx_r0.error.message, " ");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", ctx_r0.error.details, " ");
} }
class ServerErrorComponent {
    constructor(router) {
        var _a, _b;
        this.router = router;
        const navigation = router.getCurrentNavigation();
        this.error = (_b = (_a = navigation === null || navigation === void 0 ? void 0 : navigation.extras) === null || _a === void 0 ? void 0 : _a.state) === null || _b === void 0 ? void 0 : _b.error;
    }
    ngOnInit() {
    }
}
ServerErrorComponent.ɵfac = function ServerErrorComponent_Factory(t) { return new (t || ServerErrorComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_1__.Router)); };
ServerErrorComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: ServerErrorComponent, selectors: [["app-server-error"]], decls: 105, vars: 1, consts: [[1, "main-error-page"], ["xmlns", "http://www.w3.org/2000/svg", 0, "xmlns", "xlink", "http://www.w3.org/1999/xlink", "width", "auto", "height", "290", "viewBox", "0 0 470 290"], ["id", "prefix__a", "d", "M221.677 31.582c-55.54 19.895-112.156 36.51-167.848 55.993C33.02 94.854 8.92 106.19 5.062 129.52c-2.513 15.198 5.634 31.108 17.898 38.96 5.99 3.835 13.34 6.531 16.472 13.261 4.87 10.459-3.88 22.449-13.113 28.531-9.236 6.083-20.5 10.905-24.704 21.695-1.121 2.876-1.61 5.944-1.615 9.05v.082c.006 4.547 1.05 9.169 2.674 13.386 10.06 26.125 39.391 37.567 65.477 36.17 26.086-1.398 50.827-12.414 76.417-18.085 43.668-9.678 88.389-3.63 133.098 2.418 45.256 6.122 90.501 12.244 134.63 2.054 13.65-3.153 27.4-8.086 37.944-17.999 11.883-11.174 18.63-28.03 19.648-45.048.069-1.15.106-2.31.112-3.477v-.557c-.067-15.128-5.551-31.08-17.856-37.794-7.833-4.274-17.195-4.1-25.7-1.775-5.43 1.485-10.766 3.811-16.368 3.85-5.601.04-11.763-3-13.386-8.812-1.707-6.11 2.182-12.416 6.642-16.586 9.073-8.477 21.203-12.714 29.44-22.137 7.927-9.07 11.265-22.586 8.575-34.734-2.692-12.149-11.326-22.552-22.189-26.73-14.224-5.471-29.105-.465-44.051 4.542-14.139 4.736-28.34 9.474-42.1 5.336-2.1-.63-4.312-1.606-5.418-3.643-1.08-1.989-.835-4.511-.214-6.719 3.468-12.355 16.938-20.22 17.528-33.12.322-7.01-3.504-13.572-8.325-18.26C327.188 4.247 316.599.782 305.483.782c-28.103 0-59.605 22.132-83.806 30.8z"], ["id", "prefix__c", "d", "M39.89.625c-2.162 6.638-7.77 12.79-13.571 16.612-9.236 6.082-20.5 10.904-24.704 21.694C.494 41.807.005 44.875 0 47.98v.082c.006 4.547 1.05 9.17 2.674 13.386 10.06 26.126 39.391 37.568 65.477 36.17 26.086-1.397 50.827-12.414 76.417-18.085 43.668-9.678 88.389-3.629 133.098 2.419 45.256 6.122 90.501 12.244 134.63 2.054 13.65-3.154 27.4-8.086 37.944-18 11.883-11.174 18.63-28.03 19.648-45.048.069-1.15.106-2.31.112-3.477v-.557c-.025-5.484-.761-11.079-2.277-16.3H39.89z"], ["id", "prefix__e", "d", "M.99 2.778v5.157c0 1.457 1.467 2.638 3.277 2.638s3.278-1.181 3.278-2.638V2.778c0-1.457-1.467-2.637-3.278-2.637C2.457.141.99 1.321.99 2.778z"], ["id", "prefix__g", "d", "M.577 2.778v5.157c0 1.457 1.468 2.638 3.279 2.638 1.81 0 3.278-1.181 3.278-2.638V2.778c0-1.457-1.468-2.637-3.278-2.637S.576 1.321.576 2.778z"], ["id", "prefix__i", "d", "M.39 2.037c0 1.08.88 1.956 1.966 1.956 1.087 0 1.968-.877 1.968-1.956 0-1.08-.88-1.956-1.968-1.956C1.27.081.39.957.39 2.037z"], ["id", "prefix__k", "d", "M.39 2.466c0 1.08.88 1.956 1.966 1.956 1.087 0 1.968-.875 1.968-1.956 0-1.079-.88-1.954-1.968-1.954C1.27.512.39 1.387.39 2.466z"], ["id", "prefix__m", "d", "M1.963.022C.893.022.026.898.026 1.977c0 1.08.867 1.956 1.937 1.956H16.45c1.068 0 1.935-.876 1.935-1.956 0-1.079-.867-1.955-1.935-1.955H1.963z"], ["id", "prefix__o", "d", "M5.214 5.34c-.85 3.38 1.072 5.145 5.14 11.15L.464 55.893l44.419 11.492 9.888-39.401c6.384-3.286 8.919-3.909 9.77-7.295.793-3.159-1.093-6.383-4.206-7.189L12.294 1.071c-.48-.125-.962-.185-1.437-.185-2.598 0-4.972 1.783-5.643 4.453z"], ["id", "prefix__q", "d", "M5 6.465c-1.055 4.052 1.213 6.332 4.666 11.2l-9.2 35.38 2.075.52 9.442-36.306c-3.886-5.53-5.717-7.165-4.909-10.276.758-2.91 3.791-4.675 6.762-3.933l45.868 11.453c2.971.742 4.772 3.714 4.015 6.624-.81 3.118-3.222 3.687-9.328 6.72L44.95 64.154l2.076.518 9.2-35.38c5.404-2.654 8.514-3.59 9.57-7.646 1.047-4.032-1.447-8.15-5.563-9.177L14.365 1.016c-.635-.159-1.273-.234-1.9-.234-3.44 0-6.58 2.274-7.465 5.683z"], ["id", "prefix__s", "d", "M5.916.737C2.89.737.428 3.265.428 6.37c0 3.326 2.16 4.498 7.25 9.07v32.236h43.2V15.44c5.077-4.56 7.25-5.74 7.25-9.07 0-3.105-2.462-5.633-5.488-5.633H5.916z"], ["id", "prefix__u", "d", "M7.721.63C3.638.63.314 4.073.314 8.3c0 4.249 2.667 5.935 7.065 9.878v30.697h2.058V17.21C4.476 12.72 2.375 11.562 2.375 8.3c0-3.05 2.398-5.535 5.346-5.535h45.511c2.949 0 5.347 2.484 5.347 5.534 0 3.269-2.108 4.422-7.062 8.911v31.665h2.06V18.178c4.389-3.939 7.061-5.626 7.061-9.879 0-4.227-3.32-7.668-7.406-7.668H7.722z"], ["id", "prefix__w", "d", "M4.278.171C2.09.171.314 1.931.314 4.097v43c0 2.168 1.776 3.926 3.964 3.926H69.79c2.187 0 3.963-1.758 3.963-3.926v-43C73.753 1.93 71.977.17 69.79.17H4.278z"], ["id", "prefix__y", "d", "M.102 2.593C-.017 3.898.95 5.053 2.26 5.177c1.308.122 2.469-.835 2.591-2.138C4.97 1.735 4.007.58 2.696.456 2.62.448 2.544.445 2.47.445 1.253.445.217 1.367.102 2.593z"], ["id", "prefix__A", "d", "M.007 2.613c-.119 1.303.844 2.459 2.155 2.582 1.311.122 2.472-.834 2.593-2.138.12-1.304-.843-2.46-2.154-2.582-.077-.009-.153-.011-.227-.011-1.216 0-2.253.92-2.367 2.149z"], ["id", "prefix__C", "d", "M.79 3.73c-.144.275.15.627.337.37C3.783.462 7.604.373 9.772 4.237c.244.43.852-.108.639-.557C9.353 1.496 7.533.36 5.676.36 3.852.36 1.994 1.456.79 3.73z"], ["id", "prefix__E", "d", "M8.699 4.235L2.878 2.667c-.528-.144-1.074.182-1.219.725-.143.547.167 1.102.693 1.243l5.821 1.572-1.076 4.048-5.821-1.572c-.528-.141-1.073.183-1.217.729-.145.544.164 1.1.692 1.24l5.822 1.573-.9 3.377 7.313 1.973c4.037 1.088 8.21-1.407 9.32-5.573.753-2.832-.102-5.71-1.996-7.605-.893-.891-2.014-1.564-3.307-1.914L9.69.511 8.7 4.235z"], ["id", "prefix__G", "d", "M1.203 6.693c-1.123 4.183 1.356 8.479 5.536 9.596l7.57 2.024 1.534-5.714 2.531-9.428-7.569-2.025C10.128.966 9.445.88 8.775.88c-3.462 0-6.632 2.31-7.572 5.814z"], ["fill", "none", "fill-rule", "evenodd"], ["fill", "#FFF", "d", "M0 0H1366V800H0z", "transform", "translate(-448 -158)"], ["transform", "translate(-448 -158) translate(448 157) translate(0 .61)"], ["id", "prefix__b", "fill", "#fff"], [0, "xlink", "href", "#prefix__a"], ["fill", "#F6F6F7", "d", "M-5.995 296.763L475.995 296.763 475.995 -5.213 -5.995 -5.213z", "mask", "url(#prefix__b)"], ["transform", "translate(-448 -158) translate(448 157) translate(0 193.645)"], ["id", "prefix__d", "fill", "#fff"], [0, "xlink", "href", "#prefix__c"], ["fill", "#EDEDF0", "d", "M-20.194 115.101L487.354 115.101 487.354 36.101 -20.194 36.101z", "mask", "url(#prefix__d)"], ["stroke", "#2D3F5B", "stroke-linecap", "round", "stroke-width", "4", "d", "M42.92 106.015c1.69-.15 3.386-.39 5.08-.454 1.496-.056 3.045.109 4.354.832 2.559 1.415 3.61 4.459 6.173 5.892 3.477 1.945 7.237 2.323 11.14 2.395 4.592.085 9.185.091 13.777.069 9.192-.045 18.386-.2 27.579.004 1.455.032 2.97.052 4.292-.562 2.606-1.208 2.6-4.556-.086-5.63-1.088-.434-2.291-.441-3.463-.441l-32.929-.022c-2.27-.001-5.629.548-5.724 3.443-.215 6.553.078 15.199.713 21.722.17 1.76.492 3.722 1.92 4.769.896.657 2.057.81 3.166.904 6.696.573 13.43-.22 20.15-.185 6.721.034 13.7.992 19.326 4.657 10.875 7.087 11.581 26.256-1.336 31.914-6.591 2.887-14.242 2.35-21.267 2.257-7.79-.103-12.714.024-20.504-.132-1.647-.034-3.648-.288-4.304-1.797-.574-1.324.355-2.913 1.64-3.573 1.285-.663 2.802-.642 4.248-.603 32.437.865 62.03 4.399 94.49 2.362 15.62-.979 26.11-9.971 29.058-25.472 2.41-12.67-.035-28.363-10.01-37.409-1.853-1.68-3.952-3.08-6.24-4.098-9.326-4.147-21.037-3.107-28.714 3.924-3.952 3.619-6.283 8.75-7.291 14.006-1.948 10.149-2.077 21.397 1.549 31.197 4.503 12.17 15.295 17.937 27.852 19.106 22.936 2.138 46.167 2.247 68.908-1.419 6.817-1.1 14.77-3.416 19.496-8.795 11.362-12.935 9.584-40.905-1.435-53.582-9.245-10.635-26.641-9.576-36.17.345-4.034 4.198-6.308 9.85-7.244 15.591-2.07 12.71-.474 27.655 8.267 37.863 5.498 6.42 14.934 9.316 22.9 11.018 5.7 1.217 11.59 1.935 17.404 1.17 5.668-.749 11.264-3.322 17.003-1.433 3.896 1.282 7.194 5.378 6.09 9.328-1.146 4.102-6.015 5.773-10.238 6.335-17.139 2.28-34.126-2.651-51.125 1.61-3.002.753-5.983 1.835-8.431 3.728-2.449 1.894-4.32 4.697-4.543 7.785-.835 11.582 23.435 6.912 30.629 10.509", "transform", "translate(-448 -158) translate(448 157)", 1, "fio-500"], ["transform", "translate(-448 -158) translate(448 157) translate(389.862 231.813)"], ["id", "prefix__n", "fill", "#fff"], [0, "xlink", "href", "#prefix__m"], ["fill", "#4B4B62", "d", "M-5.969 9.929L24.38 9.929 24.38 -5.973 -5.969 -5.973z", "mask", "url(#prefix__n)", 1, "fio"], ["stroke", "#2D3F5B", "stroke-linecap", "round", "stroke-width", "4", "d", "M332.999 253.51c-7.766-.244-14.938 4.491-22.688 4.744-2.34.077-4.8.01-6.847-1.128-2.662-1.483-3.966-4.531-3.511-7.501.564-3.683 3.808-5.906 6.02-8.612 3.643-4.453 2.47-9.05-2.76-11.169-5.66-2.293-11.803-3.685-17.833-4.585", "transform", "translate(-448 -158) translate(448 157)", 1, "fio"], [1, "full-torradeira"], ["transform", "translate(-448 -158) translate(448 157) translate(332.51 262.987)"], ["id", "prefix__f", "fill", "#fff"], [0, "xlink", "href", "#prefix__e"], ["fill", "#4B4B62", "d", "M-5.006 16.566L13.541 16.566 13.541 -5.855 -5.006 -5.855z", "mask", "url(#prefix__f)"], ["transform", "translate(-448 -158) translate(448 157) translate(384.066 262.987)"], ["id", "prefix__h", "fill", "#fff"], [0, "xlink", "href", "#prefix__g"], ["fill", "#4B4B62", "d", "M-5.418 16.566L13.129 16.566 13.129 -5.855 -5.418 -5.855z", "mask", "url(#prefix__h)"], ["transform", "translate(-448 -158) translate(448 157) translate(396.056 247.4)"], ["id", "prefix__j", "fill", "#fff"], [0, "xlink", "href", "#prefix__i"], ["fill", "#4B4B62", "d", "M-5.605 9.989L10.318 9.989 10.318 -5.913 -5.605 -5.913z", "mask", "url(#prefix__j)"], ["transform", "translate(-448 -158) translate(448 157) translate(396.056 254.793)"], ["id", "prefix__l", "fill", "#fff"], [0, "xlink", "href", "#prefix__k"], ["fill", "#4B4B62", "d", "M-5.605 10.418L10.318 10.418 10.318 -5.483 -5.605 -5.483z", "mask", "url(#prefix__l)"], [1, "pao-atras"], ["transform", "translate(-448 -158) translate(448 157) translate(340.903 169.665)"], ["id", "prefix__p", "fill", "#fff"], [0, "xlink", "href", "#prefix__o"], ["fill", "#FFDA7F", "d", "M-5.53 73.379L70.718 73.379 70.718 -5.109 -5.53 -5.109z", "mask", "url(#prefix__p)"], ["transform", "translate(-448 -158) translate(448 157) translate(340.903 168.466)"], ["id", "prefix__r", "fill", "#fff"], [0, "xlink", "href", "#prefix__q"], ["fill", "#E6A95F", "d", "M-5.53 70.667L72.03 70.667 72.03 -5.213 -5.53 -5.213z", "mask", "url(#prefix__r)"], [1, "pao-frente"], ["transform", "translate(-448 -158) translate(448 157) translate(326.515 182.854)"], ["id", "prefix__t", "fill", "#fff"], [0, "xlink", "href", "#prefix__s"], ["fill", "#FFDA7F", "d", "M-5.568 53.67L64.123 53.67 64.123 -5.259 -5.568 -5.259z", "mask", "url(#prefix__t)"], ["transform", "translate(-448 -158) translate(448 157) translate(325.316 181.655)"], ["id", "prefix__v", "fill", "#fff"], [0, "xlink", "href", "#prefix__u"], ["fill", "#E6A95F", "d", "M-5.68 54.87L66.634 54.87 66.634 -5.363 -5.68 -5.363z", "mask", "url(#prefix__v)"], ["transform", "translate(-448 -158) translate(448 157) translate(344.301 197.242)"], ["id", "prefix__z", "fill", "#fff"], [0, "xlink", "href", "#prefix__y"], ["fill", "#633", "d", "M-4.153 -6.995L12.298 -3.797 9.107 12.627 -7.346 9.429z", "mask", "url(#prefix__z)", 1, "olho-esq"], ["transform", "translate(-448 -158) translate(448 157) translate(361.087 197.242)"], ["id", "prefix__B", "fill", "#fff"], [0, "xlink", "href", "#prefix__A"], ["fill", "#633", "d", "M-4.248 -6.976L12.203 -3.778 9.011 12.645 -7.441 9.448z", "mask", "url(#prefix__B)", 1, "olho-dir"], ["transform", "translate(-448 -158) translate(448 157) translate(350.495 206.834)"], ["id", "prefix__D", "fill", "#fff"], [0, "xlink", "href", "#prefix__C"], ["fill", "#633", "d", "M-3.14 -7.546L17.993 -3.438 14.8 12.985 -6.333 8.878z", "mask", "url(#prefix__D)", 1, "boca"], ["transform", "translate(-448 -158) translate(448 157) translate(325.316 218.624)", 1, "torradeira"], ["id", "prefix__x", "fill", "#fff"], [0, "xlink", "href", "#prefix__w"], ["fill", "#C81414", "d", "M-5.68 57.017L79.748 57.017 79.748 -5.823 -5.68 -5.823z", "mask", "url(#prefix__x)"], ["stroke", "#fdba55", "stroke-linecap", "round", "stroke-width", "1.8", 1, "raios"], ["d", "M18.393 5.769L21.35 2.82M9.304 4.452L7.306.783M14.502 3.516L14.981.207M3.272 28.271L.229 31.133M12.317 29.853L14.208 33.577M7.095 30.637L6.52 33.93", "transform", "translate(-448 -158) translate(448 157) translate(260.571 203.237)"], ["transform", "translate(-448 -158) translate(448 157) translate(272.561 215.226)"], ["id", "prefix__F", "fill", "#fff"], [0, "xlink", "href", "#prefix__E"], ["fill", "#2D3F5B", "d", "M10.383 -14.007L34.351 9.962 12.875 31.438 -11.095 7.469z", "mask", "url(#prefix__F)", 1, "tomada"], ["transform", "translate(-448 -158) translate(448 157) translate(246.184 208.033)"], ["id", "prefix__H", "fill", "#fff"], [0, "xlink", "href", "#prefix__G"], ["fill", "#2D3F5B", "d", "M10.807 -12.875L32.391 8.709 10.793 30.309 -10.793 8.724z", "mask", "url(#prefix__H)", 1, "tomada"], [1, "error-title"], [1, "error-subtitle"], [4, "ngIf"], [1, "text-danger"], [1, "mt-4"]], template: function ServerErrorComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnamespaceSVG"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "svg", 1)(2, "defs");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](3, "path", 2)(4, "path", 3)(5, "path", 4)(6, "path", 5)(7, "path", 6)(8, "path", 7)(9, "path", 8)(10, "path", 9)(11, "path", 10)(12, "path", 11)(13, "path", 12)(14, "path", 13)(15, "path", 14)(16, "path", 15)(17, "path", 16)(18, "path", 17)(19, "path", 18);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](20, "g", 19);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](21, "path", 20);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](22, "g")(23, "g", 21)(24, "mask", 22);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](25, "use", 23);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](26, "path", 24);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](27, "g", 25)(28, "mask", 26);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](29, "use", 27);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](30, "path", 28);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](31, "path", 29);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](32, "g", 30)(33, "mask", 31);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](34, "use", 32);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](35, "path", 33);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](36, "path", 34);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](37, "g", 35)(38, "g", 36)(39, "mask", 37);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](40, "use", 38);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](41, "path", 39);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](42, "g", 40)(43, "mask", 41);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](44, "use", 42);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](45, "path", 43);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](46, "g", 44)(47, "mask", 45);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](48, "use", 46);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](49, "path", 47);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](50, "g", 48)(51, "mask", 49);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](52, "use", 50);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](53, "path", 51);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](54, "g", 52)(55, "g", 53)(56, "mask", 54);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](57, "use", 55);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](58, "path", 56);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](59, "g", 57)(60, "mask", 58);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](61, "use", 59);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](62, "path", 60);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](63, "g", 61)(64, "g", 62)(65, "mask", 63);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](66, "use", 64);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](67, "path", 65);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](68, "g", 66)(69, "mask", 67);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](70, "use", 68);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](71, "path", 69);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](72, "g", 70)(73, "mask", 71);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](74, "use", 72);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](75, "path", 73);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](76, "g", 74)(77, "mask", 75);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](78, "use", 76);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](79, "path", 77);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](80, "g", 78)(81, "mask", 79);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](82, "use", 80);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](83, "path", 81);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](84, "g", 82)(85, "mask", 83);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](86, "use", 84);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](87, "path", 85);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](88, "g", 86);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](89, "path", 87);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](90, "g", 88)(91, "mask", 89);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](92, "use", 90);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](93, "path", 91);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](94, "g", 92)(95, "mask", 93);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](96, "use", 94);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](97, "path", 95);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()()()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnamespaceHTML"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](98, "h1", 96);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](99, " Woops! ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelement"](100, "br");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](101, "Something went wrong :( ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](102, "h2", 97);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](103, " Have you tried turning it off and on again? ");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](104, ServerErrorComponent_ng_container_104_Template, 5, 2, "ng-container", 98);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](104);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.error);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_2__.NgIf], styles: ["@import url('https://fonts.googleapis.com/css2?family=Roboto&display=swap');\r\n\r\n.main-error-page[_ngcontent-%COMP%] {\r\n  min-height: 600px;\r\n  margin: 0px auto;\r\n  width: auto;\r\n  max-width: 560px;\r\n  display: flex;\r\n  align-items: center;\r\n  justify-content: center;\r\n  flex-direction: column;\r\n}\r\n\r\n.error-title[_ngcontent-%COMP%] {\r\n  max-width: 529px;\r\n  font-family: Roboto;\r\n  font-size: 38px;\r\n  font-weight: bold;\r\n  font-stretch: normal;\r\n  font-style: normal;\r\n  line-height: normal;\r\n  letter-spacing: normal;\r\n  text-align: center;\r\n  color: #4b4b62;\r\n  margin-bottom: 16px;\r\n}\r\n\r\n.error-subtitle[_ngcontent-%COMP%] {\r\n  max-width: 568px;\r\n  font-family: Roboto;\r\n  font-size: 16px;\r\n  font-weight: normal;\r\n  font-stretch: normal;\r\n  font-style: normal;\r\n  line-height: 1.31;\r\n  letter-spacing: normal;\r\n  text-align: center;\r\n  color: #4b4b62;\r\n  margin-bottom: 24px;\r\n}\r\n\r\nsvg[_ngcontent-%COMP%] {\r\n  margin-bottom: 16px;\r\n}\r\n\r\n.full-torradeira[_ngcontent-%COMP%] {\r\n}\r\n\r\n.torradeira[_ngcontent-%COMP%] {\r\n}\r\n\r\n.pao-atras[_ngcontent-%COMP%] {\r\n  animation: leftright 1s alternate infinite;\r\n  transform-origin: center;\r\n}\r\n\r\n.pao-frente[_ngcontent-%COMP%] {\r\n  animation: leftright 1s 0.3s alternate infinite;\r\n  transform-origin: center;\r\n}\r\n\r\n.olho-esq[_ngcontent-%COMP%] {\r\n  animation: sad 2s alternate infinite;\r\n  transform-origin: center;\r\n}\r\n\r\n.olho-dir[_ngcontent-%COMP%] {\r\n  animation: sad 2s alternate infinite;\r\n  transform-origin: center;\r\n}\r\n\r\n.boca[_ngcontent-%COMP%] {\r\n  animation: sad 2s alternate infinite;\r\n  transform-origin: center;\r\n}\r\n\r\n.raios[_ngcontent-%COMP%] {\r\n  animation: flicker-4 4s linear infinite both;\r\n}\r\n\r\n.tomada[_ngcontent-%COMP%] {\r\n  animation: vibrate-1 3s linear infinite both;\r\n}\r\n\r\n.fio-500[_ngcontent-%COMP%] {\r\n  animation: vibrate-1 3s linear infinite both;\r\n}\r\n\r\n.fio[_ngcontent-%COMP%] {\r\n  animation: vibrate-1 3s linear infinite both;\r\n}\r\n\r\n@keyframes scales {\r\n  from {\r\n    transform: scale(0.98);\r\n  }\r\n\r\n  to {\r\n    transform: scale(1);\r\n  }\r\n}\r\n\r\n\r\n\r\n\r\n\r\n@keyframes flicker-4 {\r\n  0%, 100% {\r\n    opacity: 1;\r\n  }\r\n\r\n  31.98% {\r\n    opacity: 1;\r\n  }\r\n\r\n  32% {\r\n    opacity: 0;\r\n  }\r\n\r\n  32.8% {\r\n    opacity: 0;\r\n  }\r\n\r\n  32.82% {\r\n    opacity: 1;\r\n  }\r\n\r\n  34.98% {\r\n    opacity: 1;\r\n  }\r\n\r\n  35% {\r\n    opacity: 0;\r\n  }\r\n\r\n  35.7% {\r\n    opacity: 0;\r\n  }\r\n\r\n  35.72% {\r\n    opacity: 1;\r\n  }\r\n\r\n  36.98% {\r\n    opacity: 1;\r\n  }\r\n\r\n  37% {\r\n    opacity: 0;\r\n  }\r\n\r\n  37.6% {\r\n    opacity: 0;\r\n  }\r\n\r\n  37.62% {\r\n    opacity: 1;\r\n  }\r\n\r\n  67.98% {\r\n    opacity: 1;\r\n  }\r\n\r\n  68% {\r\n    opacity: 0;\r\n  }\r\n\r\n  68.4% {\r\n    opacity: 0;\r\n  }\r\n\r\n  68.42% {\r\n    opacity: 1;\r\n  }\r\n\r\n  95.98% {\r\n    opacity: 1;\r\n  }\r\n\r\n  96% {\r\n    opacity: 0;\r\n  }\r\n\r\n  96.7% {\r\n    opacity: 0;\r\n  }\r\n\r\n  96.72% {\r\n    opacity: 1;\r\n  }\r\n\r\n  98.98% {\r\n    opacity: 1;\r\n  }\r\n\r\n  99% {\r\n    opacity: 0;\r\n  }\r\n\r\n  99.6% {\r\n    opacity: 0;\r\n  }\r\n\r\n  99.62% {\r\n    opacity: 1;\r\n  }\r\n}\r\n\r\n\r\n\r\n\r\n\r\n@keyframes vibrate-1 {\r\n  0% {\r\n    transform: translate(0);\r\n  }\r\n\r\n  20% {\r\n    transform: translate(-2px, 2px);\r\n  }\r\n\r\n  40% {\r\n    transform: translate(-2px, -2px);\r\n  }\r\n\r\n  60% {\r\n    transform: translate(2px, 2px);\r\n  }\r\n\r\n  80% {\r\n    transform: translate(2px, -2px);\r\n  }\r\n\r\n  100% {\r\n    transform: translate(0);\r\n  }\r\n}\r\n\r\n\r\n\r\n\r\n\r\n@keyframes sad {\r\n  0% {\r\n    transform: rotateX(0deg) rotateY(0deg);\r\n  }\r\n\r\n  100% {\r\n    transform: rotateX(10deg) rotateY(5deg);\r\n  }\r\n}\r\n\r\n@keyframes leftright {\r\n\r\n  0% {\r\n    transform: rotateZ( 0deg)\r\n  }\r\n\r\n  100% {\r\n    transform: rotateZ( -15deg)\r\n  }\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInNlcnZlci1lcnJvci5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBLDJFQUEyRTs7QUFFM0U7RUFDRSxpQkFBaUI7RUFDakIsZ0JBQWdCO0VBQ2hCLFdBQVc7RUFDWCxnQkFBZ0I7RUFDaEIsYUFBYTtFQUNiLG1CQUFtQjtFQUNuQix1QkFBdUI7RUFDdkIsc0JBQXNCO0FBQ3hCOztBQUVBO0VBQ0UsZ0JBQWdCO0VBQ2hCLG1CQUFtQjtFQUNuQixlQUFlO0VBQ2YsaUJBQWlCO0VBQ2pCLG9CQUFvQjtFQUNwQixrQkFBa0I7RUFDbEIsbUJBQW1CO0VBQ25CLHNCQUFzQjtFQUN0QixrQkFBa0I7RUFDbEIsY0FBYztFQUNkLG1CQUFtQjtBQUNyQjs7QUFFQTtFQUNFLGdCQUFnQjtFQUNoQixtQkFBbUI7RUFDbkIsZUFBZTtFQUNmLG1CQUFtQjtFQUNuQixvQkFBb0I7RUFDcEIsa0JBQWtCO0VBQ2xCLGlCQUFpQjtFQUNqQixzQkFBc0I7RUFDdEIsa0JBQWtCO0VBQ2xCLGNBQWM7RUFDZCxtQkFBbUI7QUFDckI7O0FBRUE7RUFDRSxtQkFBbUI7QUFDckI7O0FBR0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0VBQ0UsMENBQTBDO0VBQzFDLHdCQUF3QjtBQUMxQjs7QUFFQTtFQUNFLCtDQUErQztFQUMvQyx3QkFBd0I7QUFDMUI7O0FBRUE7RUFDRSxvQ0FBb0M7RUFDcEMsd0JBQXdCO0FBQzFCOztBQUVBO0VBQ0Usb0NBQW9DO0VBQ3BDLHdCQUF3QjtBQUMxQjs7QUFFQTtFQUNFLG9DQUFvQztFQUNwQyx3QkFBd0I7QUFDMUI7O0FBRUE7RUFFRSw0Q0FBNEM7QUFDOUM7O0FBRUE7RUFFRSw0Q0FBNEM7QUFDOUM7O0FBRUE7RUFFRSw0Q0FBNEM7QUFDOUM7O0FBRUE7RUFFRSw0Q0FBNEM7QUFDOUM7O0FBRUE7RUFDRTtJQUNFLHNCQUFzQjtFQUN4Qjs7RUFFQTtJQUNFLG1CQUFtQjtFQUNyQjtBQUNGOztBQUVBOzs7OzttREFLbUQ7O0FBRW5EOzs7O0VBSUU7O0FBdUdGO0VBQ0U7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7O0VBRUE7SUFDRSxVQUFVO0VBQ1o7QUFDRjs7QUFHQTs7Ozs7bURBS21EOztBQUVuRDs7OztFQUlFOztBQWlDRjtFQUNFO0lBRUUsdUJBQXVCO0VBQ3pCOztFQUVBO0lBRUUsK0JBQStCO0VBQ2pDOztFQUVBO0lBRUUsZ0NBQWdDO0VBQ2xDOztFQUVBO0lBRUUsOEJBQThCO0VBQ2hDOztFQUVBO0lBRUUsK0JBQStCO0VBQ2pDOztFQUVBO0lBRUUsdUJBQXVCO0VBQ3pCO0FBQ0Y7O0FBSUE7Ozs7O21EQUttRDs7QUFFbkQ7Ozs7RUFJRTs7QUFvQ0Y7RUFDRTtJQUNFLHNDQUFzQztFQUN4Qzs7RUFFQTtJQUNFLHVDQUF1QztFQUN6QztBQUNGOztBQUVBOztFQUVFO0lBQ0U7RUFDRjs7RUFFQTtJQUNFO0VBQ0Y7QUFDRiIsImZpbGUiOiJzZXJ2ZXItZXJyb3IuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIkBpbXBvcnQgdXJsKCdodHRwczovL2ZvbnRzLmdvb2dsZWFwaXMuY29tL2NzczI/ZmFtaWx5PVJvYm90byZkaXNwbGF5PXN3YXAnKTtcclxuXHJcbi5tYWluLWVycm9yLXBhZ2Uge1xyXG4gIG1pbi1oZWlnaHQ6IDYwMHB4O1xyXG4gIG1hcmdpbjogMHB4IGF1dG87XHJcbiAgd2lkdGg6IGF1dG87XHJcbiAgbWF4LXdpZHRoOiA1NjBweDtcclxuICBkaXNwbGF5OiBmbGV4O1xyXG4gIGFsaWduLWl0ZW1zOiBjZW50ZXI7XHJcbiAganVzdGlmeS1jb250ZW50OiBjZW50ZXI7XHJcbiAgZmxleC1kaXJlY3Rpb246IGNvbHVtbjtcclxufVxyXG5cclxuLmVycm9yLXRpdGxlIHtcclxuICBtYXgtd2lkdGg6IDUyOXB4O1xyXG4gIGZvbnQtZmFtaWx5OiBSb2JvdG87XHJcbiAgZm9udC1zaXplOiAzOHB4O1xyXG4gIGZvbnQtd2VpZ2h0OiBib2xkO1xyXG4gIGZvbnQtc3RyZXRjaDogbm9ybWFsO1xyXG4gIGZvbnQtc3R5bGU6IG5vcm1hbDtcclxuICBsaW5lLWhlaWdodDogbm9ybWFsO1xyXG4gIGxldHRlci1zcGFjaW5nOiBub3JtYWw7XHJcbiAgdGV4dC1hbGlnbjogY2VudGVyO1xyXG4gIGNvbG9yOiAjNGI0YjYyO1xyXG4gIG1hcmdpbi1ib3R0b206IDE2cHg7XHJcbn1cclxuXHJcbi5lcnJvci1zdWJ0aXRsZSB7XHJcbiAgbWF4LXdpZHRoOiA1NjhweDtcclxuICBmb250LWZhbWlseTogUm9ib3RvO1xyXG4gIGZvbnQtc2l6ZTogMTZweDtcclxuICBmb250LXdlaWdodDogbm9ybWFsO1xyXG4gIGZvbnQtc3RyZXRjaDogbm9ybWFsO1xyXG4gIGZvbnQtc3R5bGU6IG5vcm1hbDtcclxuICBsaW5lLWhlaWdodDogMS4zMTtcclxuICBsZXR0ZXItc3BhY2luZzogbm9ybWFsO1xyXG4gIHRleHQtYWxpZ246IGNlbnRlcjtcclxuICBjb2xvcjogIzRiNGI2MjtcclxuICBtYXJnaW4tYm90dG9tOiAyNHB4O1xyXG59XHJcblxyXG5zdmcge1xyXG4gIG1hcmdpbi1ib3R0b206IDE2cHg7XHJcbn1cclxuXHJcblxyXG4uZnVsbC10b3JyYWRlaXJhIHtcclxufVxyXG5cclxuLnRvcnJhZGVpcmEge1xyXG59XHJcblxyXG4ucGFvLWF0cmFzIHtcclxuICBhbmltYXRpb246IGxlZnRyaWdodCAxcyBhbHRlcm5hdGUgaW5maW5pdGU7XHJcbiAgdHJhbnNmb3JtLW9yaWdpbjogY2VudGVyO1xyXG59XHJcblxyXG4ucGFvLWZyZW50ZSB7XHJcbiAgYW5pbWF0aW9uOiBsZWZ0cmlnaHQgMXMgMC4zcyBhbHRlcm5hdGUgaW5maW5pdGU7XHJcbiAgdHJhbnNmb3JtLW9yaWdpbjogY2VudGVyO1xyXG59XHJcblxyXG4ub2xoby1lc3Ege1xyXG4gIGFuaW1hdGlvbjogc2FkIDJzIGFsdGVybmF0ZSBpbmZpbml0ZTtcclxuICB0cmFuc2Zvcm0tb3JpZ2luOiBjZW50ZXI7XHJcbn1cclxuXHJcbi5vbGhvLWRpciB7XHJcbiAgYW5pbWF0aW9uOiBzYWQgMnMgYWx0ZXJuYXRlIGluZmluaXRlO1xyXG4gIHRyYW5zZm9ybS1vcmlnaW46IGNlbnRlcjtcclxufVxyXG5cclxuLmJvY2Ege1xyXG4gIGFuaW1hdGlvbjogc2FkIDJzIGFsdGVybmF0ZSBpbmZpbml0ZTtcclxuICB0cmFuc2Zvcm0tb3JpZ2luOiBjZW50ZXI7XHJcbn1cclxuXHJcbi5yYWlvcyB7XHJcbiAgLXdlYmtpdC1hbmltYXRpb246IGZsaWNrZXItNCA0cyBsaW5lYXIgaW5maW5pdGUgYm90aDtcclxuICBhbmltYXRpb246IGZsaWNrZXItNCA0cyBsaW5lYXIgaW5maW5pdGUgYm90aDtcclxufVxyXG5cclxuLnRvbWFkYSB7XHJcbiAgLXdlYmtpdC1hbmltYXRpb246IHZpYnJhdGUtMSAzcyBsaW5lYXIgaW5maW5pdGUgYm90aDtcclxuICBhbmltYXRpb246IHZpYnJhdGUtMSAzcyBsaW5lYXIgaW5maW5pdGUgYm90aDtcclxufVxyXG5cclxuLmZpby01MDAge1xyXG4gIC13ZWJraXQtYW5pbWF0aW9uOiB2aWJyYXRlLTEgM3MgbGluZWFyIGluZmluaXRlIGJvdGg7XHJcbiAgYW5pbWF0aW9uOiB2aWJyYXRlLTEgM3MgbGluZWFyIGluZmluaXRlIGJvdGg7XHJcbn1cclxuXHJcbi5maW8ge1xyXG4gIC13ZWJraXQtYW5pbWF0aW9uOiB2aWJyYXRlLTEgM3MgbGluZWFyIGluZmluaXRlIGJvdGg7XHJcbiAgYW5pbWF0aW9uOiB2aWJyYXRlLTEgM3MgbGluZWFyIGluZmluaXRlIGJvdGg7XHJcbn1cclxuXHJcbkBrZXlmcmFtZXMgc2NhbGVzIHtcclxuICBmcm9tIHtcclxuICAgIHRyYW5zZm9ybTogc2NhbGUoMC45OCk7XHJcbiAgfVxyXG5cclxuICB0byB7XHJcbiAgICB0cmFuc2Zvcm06IHNjYWxlKDEpO1xyXG4gIH1cclxufVxyXG5cclxuLyogLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4gKiBHZW5lcmF0ZWQgYnkgQW5pbWlzdGEgb24gMjAyMC00LTEgMTQ6NTg6MTZcclxuICogTGljZW5zZWQgdW5kZXIgRnJlZUJTRCBMaWNlbnNlLlxyXG4gKiBTZWUgaHR0cDovL2FuaW1pc3RhLm5ldC9saWNlbnNlIGZvciBtb3JlIGluZm8uIFxyXG4gKiB3OiBodHRwOi8vYW5pbWlzdGEubmV0LCB0OiBAY3NzYW5pbWlzdGFcclxuICogLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLSAqL1xyXG5cclxuLyoqXHJcbiAqIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuICogYW5pbWF0aW9uIGZsaWNrZXItNFxyXG4gKiAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbiAqL1xyXG5ALXdlYmtpdC1rZXlmcmFtZXMgZmxpY2tlci00IHtcclxuICAwJSwgMTAwJSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgMzEuOTglIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICAzMiUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDMyLjglIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICAzMi44MiUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDM0Ljk4JSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgMzUlIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICAzNS43JSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgMzUuNzIlIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICAzNi45OCUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDM3JSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgMzcuNiUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDM3LjYyJSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgNjcuOTglIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICA2OCUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDY4LjQlIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICA2OC40MiUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDk1Ljk4JSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgOTYlIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICA5Ni43JSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgOTYuNzIlIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICA5OC45OCUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDk5JSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgOTkuNiUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDk5LjYyJSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxufVxyXG5cclxuQGtleWZyYW1lcyBmbGlja2VyLTQge1xyXG4gIDAlLCAxMDAlIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICAzMS45OCUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDMyJSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgMzIuOCUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDMyLjgyJSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgMzQuOTglIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICAzNSUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDM1LjclIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICAzNS43MiUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDM2Ljk4JSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgMzclIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICAzNy42JSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgMzcuNjIlIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICA2Ny45OCUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDY4JSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgNjguNCUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDY4LjQyJSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgOTUuOTglIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG5cclxuICA5NiUge1xyXG4gICAgb3BhY2l0eTogMDtcclxuICB9XHJcblxyXG4gIDk2LjclIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICA5Ni43MiUge1xyXG4gICAgb3BhY2l0eTogMTtcclxuICB9XHJcblxyXG4gIDk4Ljk4JSB7XHJcbiAgICBvcGFjaXR5OiAxO1xyXG4gIH1cclxuXHJcbiAgOTklIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG5cclxuICA5OS42JSB7XHJcbiAgICBvcGFjaXR5OiAwO1xyXG4gIH1cclxuXHJcbiAgOTkuNjIlIHtcclxuICAgIG9wYWNpdHk6IDE7XHJcbiAgfVxyXG59XHJcblxyXG5cclxuLyogLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4gKiBHZW5lcmF0ZWQgYnkgQW5pbWlzdGEgb24gMjAyMC00LTEgMTU6MTc6NTdcclxuICogTGljZW5zZWQgdW5kZXIgRnJlZUJTRCBMaWNlbnNlLlxyXG4gKiBTZWUgaHR0cDovL2FuaW1pc3RhLm5ldC9saWNlbnNlIGZvciBtb3JlIGluZm8uIFxyXG4gKiB3OiBodHRwOi8vYW5pbWlzdGEubmV0LCB0OiBAY3NzYW5pbWlzdGFcclxuICogLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLSAqL1xyXG5cclxuLyoqXHJcbiAqIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuICogYW5pbWF0aW9uIHZpYnJhdGUtMVxyXG4gKiAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbiAqL1xyXG5ALXdlYmtpdC1rZXlmcmFtZXMgdmlicmF0ZS0xIHtcclxuICAwJSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKDApO1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGUoMCk7XHJcbiAgfVxyXG5cclxuICAyMCUge1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZSgtMnB4LCAycHgpO1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGUoLTJweCwgMnB4KTtcclxuICB9XHJcblxyXG4gIDQwJSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKC0ycHgsIC0ycHgpO1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGUoLTJweCwgLTJweCk7XHJcbiAgfVxyXG5cclxuICA2MCUge1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZSgycHgsIDJweCk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZSgycHgsIDJweCk7XHJcbiAgfVxyXG5cclxuICA4MCUge1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZSgycHgsIC0ycHgpO1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGUoMnB4LCAtMnB4KTtcclxuICB9XHJcblxyXG4gIDEwMCUge1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZSgwKTtcclxuICAgIHRyYW5zZm9ybTogdHJhbnNsYXRlKDApO1xyXG4gIH1cclxufVxyXG5cclxuQGtleWZyYW1lcyB2aWJyYXRlLTEge1xyXG4gIDAlIHtcclxuICAgIC13ZWJraXQtdHJhbnNmb3JtOiB0cmFuc2xhdGUoMCk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZSgwKTtcclxuICB9XHJcblxyXG4gIDIwJSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKC0ycHgsIDJweCk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZSgtMnB4LCAycHgpO1xyXG4gIH1cclxuXHJcbiAgNDAlIHtcclxuICAgIC13ZWJraXQtdHJhbnNmb3JtOiB0cmFuc2xhdGUoLTJweCwgLTJweCk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZSgtMnB4LCAtMnB4KTtcclxuICB9XHJcblxyXG4gIDYwJSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKDJweCwgMnB4KTtcclxuICAgIHRyYW5zZm9ybTogdHJhbnNsYXRlKDJweCwgMnB4KTtcclxuICB9XHJcblxyXG4gIDgwJSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKDJweCwgLTJweCk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZSgycHgsIC0ycHgpO1xyXG4gIH1cclxuXHJcbiAgMTAwJSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlKDApO1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGUoMCk7XHJcbiAgfVxyXG59XHJcblxyXG5cclxuXHJcbi8qIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuICogR2VuZXJhdGVkIGJ5IEFuaW1pc3RhIG9uIDIwMjAtNC0xIDE1OjQyOjQ1XHJcbiAqIExpY2Vuc2VkIHVuZGVyIEZyZWVCU0QgTGljZW5zZS5cclxuICogU2VlIGh0dHA6Ly9hbmltaXN0YS5uZXQvbGljZW5zZSBmb3IgbW9yZSBpbmZvLiBcclxuICogdzogaHR0cDovL2FuaW1pc3RhLm5ldCwgdDogQGNzc2FuaW1pc3RhXHJcbiAqIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0gKi9cclxuXHJcbi8qKlxyXG4gKiAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbiAqIGFuaW1hdGlvbiB3b2JibGUtdmVyLXJpZ2h0XHJcbiAqIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuICovXHJcbkAtd2Via2l0LWtleWZyYW1lcyB3b2JibGUtdmVyLXJpZ2h0IHtcclxuICAwJSwgMTAwJSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlWSgwKSByb3RhdGUoMCk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZVkoMCkgcm90YXRlKDApO1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm0tb3JpZ2luOiA1MCUgNTAlO1xyXG4gICAgdHJhbnNmb3JtLW9yaWdpbjogNTAlIDUwJTtcclxuICB9XHJcblxyXG4gIDE1JSB7XHJcbiAgICAtd2Via2l0LXRyYW5zZm9ybTogdHJhbnNsYXRlWSgtMzBweCkgcm90YXRlKDZkZWcpO1xyXG4gICAgdHJhbnNmb3JtOiB0cmFuc2xhdGVZKC0zMHB4KSByb3RhdGUoNmRlZyk7XHJcbiAgfVxyXG5cclxuICAzMCUge1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZVkoMTVweCkgcm90YXRlKC02ZGVnKTtcclxuICAgIHRyYW5zZm9ybTogdHJhbnNsYXRlWSgxNXB4KSByb3RhdGUoLTZkZWcpO1xyXG4gIH1cclxuXHJcbiAgNDUlIHtcclxuICAgIC13ZWJraXQtdHJhbnNmb3JtOiB0cmFuc2xhdGVZKC0xNXB4KSByb3RhdGUoMy42ZGVnKTtcclxuICAgIHRyYW5zZm9ybTogdHJhbnNsYXRlWSgtMTVweCkgcm90YXRlKDMuNmRlZyk7XHJcbiAgfVxyXG5cclxuICA2MCUge1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZVkoOXB4KSByb3RhdGUoLTIuNGRlZyk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZVkoOXB4KSByb3RhdGUoLTIuNGRlZyk7XHJcbiAgfVxyXG5cclxuICA3NSUge1xyXG4gICAgLXdlYmtpdC10cmFuc2Zvcm06IHRyYW5zbGF0ZVkoLTZweCkgcm90YXRlKDEuMmRlZyk7XHJcbiAgICB0cmFuc2Zvcm06IHRyYW5zbGF0ZVkoLTZweCkgcm90YXRlKDEuMmRlZyk7XHJcbiAgfVxyXG59XHJcblxyXG5cclxuQGtleWZyYW1lcyBzYWQge1xyXG4gIDAlIHtcclxuICAgIHRyYW5zZm9ybTogcm90YXRlWCgwZGVnKSByb3RhdGVZKDBkZWcpO1xyXG4gIH1cclxuXHJcbiAgMTAwJSB7XHJcbiAgICB0cmFuc2Zvcm06IHJvdGF0ZVgoMTBkZWcpIHJvdGF0ZVkoNWRlZyk7XHJcbiAgfVxyXG59XHJcblxyXG5Aa2V5ZnJhbWVzIGxlZnRyaWdodCB7XHJcblxyXG4gIDAlIHtcclxuICAgIHRyYW5zZm9ybTogcm90YXRlWiggMGRlZylcclxuICB9XHJcblxyXG4gIDEwMCUge1xyXG4gICAgdHJhbnNmb3JtOiByb3RhdGVaKCAtMTVkZWcpXHJcbiAgfVxyXG59XHJcbiJdfQ== */"] });


/***/ }),

/***/ 6175:
/*!***********************************************************!*\
  !*** ./src/app/errors/test-error/test-error.component.ts ***!
  \***********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "TestErrorComponent": () => (/* binding */ TestErrorComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_common_http__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/common/http */ 8784);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ 6362);



function TestErrorComponent_div_11_li_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "li");
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
} if (rf & 2) {
    const error_r2 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtextInterpolate1"](" ", error_r2, " ");
} }
function TestErrorComponent_div_11_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 2)(1, "ul", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](2, TestErrorComponent_div_11_li_2_Template, 2, 1, "li", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngForOf", ctx_r0.validationErrors);
} }
class TestErrorComponent {
    constructor(http) {
        this.http = http;
        this.validationErrors = [];
    }
    ngOnInit() {
    }
    get404Error() {
        this.http.get('/api/buggy/not-found').subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
        });
    }
    get400Error() {
        this.http.get('/api/buggy/bad-request').subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
        });
    }
    get401Error() {
        this.http.get('/api/buggy/auth').subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
        });
    }
    get500Error() {
        this.http.get('/api/buggy/server-error').subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
        });
    }
    get400ValidationError() {
        this.http.post('/api/account/register', {}).subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
            this.validationErrors = error;
        });
    }
}
TestErrorComponent.ɵfac = function TestErrorComponent_Factory(t) { return new (t || TestErrorComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_angular_common_http__WEBPACK_IMPORTED_MODULE_1__.HttpClient)); };
TestErrorComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: TestErrorComponent, selectors: [["app-test-error"]], decls: 12, vars: 1, consts: [[1, "btn", "btn-outline-secondary", "me-3", 3, "click"], ["class", "row mt-4", 4, "ngIf"], [1, "row", "mt-4"], [1, "text-danger"], [4, "ngFor", "ngForOf"]], template: function TestErrorComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerStart"](0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](1, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TestErrorComponent_Template_button_click_1_listener() { return ctx.get404Error(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](2, " Test 404 Error");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](3, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TestErrorComponent_Template_button_click_3_listener() { return ctx.get401Error(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, " Test 401 Error");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TestErrorComponent_Template_button_click_5_listener() { return ctx.get400Error(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](6, " Test 400 Error");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](7, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TestErrorComponent_Template_button_click_7_listener() { return ctx.get400ValidationError(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, " Test 400 Validation Error");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "button", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵlistener"]("click", function TestErrorComponent_Template_button_click_9_listener() { return ctx.get500Error(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, " Test 500 Error");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtemplate"](11, TestErrorComponent_div_11_Template, 3, 1, "div", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementContainerEnd"]();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵadvance"](11);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵproperty"]("ngIf", ctx.validationErrors.length > 0);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_2__.NgIf, _angular_common__WEBPACK_IMPORTED_MODULE_2__.NgForOf], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJ0ZXN0LWVycm9yLmNvbXBvbmVudC5jc3MifQ== */"] });


/***/ }),

/***/ 970:
/*!********************************************!*\
  !*** ./src/app/footer/footer.component.ts ***!
  \********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "FooterComponent": () => (/* binding */ FooterComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);

class FooterComponent {
    constructor() { }
    ngOnInit() {
    }
}
FooterComponent.ɵfac = function FooterComponent_Factory(t) { return new (t || FooterComponent)(); };
FooterComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: FooterComponent, selectors: [["app-footer"]], decls: 20, vars: 0, consts: [[1, "container"], [1, "py-3", "my-4"], [1, "nav", "justify-content-center", "border-bottom", "pb-3", "mb-3"], [1, "nav-item"], ["href", "#", 1, "nav-link", "px-2", "text-muted"], [1, "text-center", "text-muted"]], template: function FooterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0)(1, "footer", 1)(2, "ul", 2)(3, "li", 3)(4, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](5, "Home");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](6, "li", 3)(7, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](8, "Features");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](9, "li", 3)(10, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](11, "Pricing");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](12, "li", 3)(13, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](14, "FAQs");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](15, "li", 3)(16, "a", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](17, "About");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](18, "p", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](19, "\u00A9 2021 Dating App, Inc");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()()();
    } }, styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJmb290ZXIuY29tcG9uZW50LmNzcyJ9 */"] });


/***/ }),

/***/ 5067:
/*!****************************************!*\
  !*** ./src/app/home/home.component.ts ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "HomeComponent": () => (/* binding */ HomeComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/router */ 2816);



class HomeComponent {
    constructor() { }
    ngOnInit() {
        /*if (this.accountService.currentUser$) {
          this.router.navigateByUrl('/users');
        }*/
    }
    registerToggle() {
    }
}
HomeComponent.ɵfac = function HomeComponent_Factory(t) { return new (t || HomeComponent)(); };
HomeComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineComponent"]({ type: HomeComponent, selectors: [["app-home"]], decls: 11, vars: 0, consts: [[1, "container"], [2, "text-align", "center"], [1, "mt-4"], [1, "lead"], [1, "text-center", "mt-4"], ["mat-raised-button", "", "color", "primary", "routerLink", "/register"]], template: function HomeComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](0, "div", 0)(1, "div", 1)(2, "div")(3, "h1");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](4, "Find Your Match");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](5, "div", 2)(6, "p", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](7, "Sign up For Free...");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementStart"](8, "div", 4)(9, "button", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵtext"](10, "Create Your Account");
        _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵelementEnd"]()()()();
    } }, directives: [_angular_material_button__WEBPACK_IMPORTED_MODULE_1__.MatButton, _angular_router__WEBPACK_IMPORTED_MODULE_2__.RouterLink], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJob21lLmNvbXBvbmVudC5jc3MifQ== */"] });


/***/ }),

/***/ 6558:
/*!******************************************!*\
  !*** ./src/app/lists/lists.component.ts ***!
  \******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "ListsComponent": () => (/* binding */ ListsComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/user.service */ 5089);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _users_user_card_user_card_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../users/user-card/user-card.component */ 9412);
/* harmony import */ var _angular_material_paginator__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/material/paginator */ 6439);





function ListsComponent_div_0_div_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](1, "app-user-card", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const user_r3 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("isLiked", true)("user", user_r3);
} }
function ListsComponent_div_0_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 2)(1, "div", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](2, ListsComponent_div_0_div_2_Template, 2, 2, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngForOf", ctx_r0.matches);
} }
function ListsComponent_div_1_Template(rf, ctx) { if (rf & 1) {
    const _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 7)(1, "mat-paginator", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("page", function ListsComponent_div_1_Template_mat_paginator_page_1_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r5); const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r4.pageChanged($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
} if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("length", ctx_r1.pagination.totalItems)("pageSize", ctx_r1.pagination.itemsPerPage);
} }
class ListsComponent {
    constructor(userService) {
        this.userService = userService;
        this.pagination = {
            itemsPerPage: 4,
            currentPage: 1,
            totalItems: 0,
            totalPages: 0
        };
        this.matches = [];
    }
    ngOnInit() {
        this.loadMatches();
    }
    loadMatches() {
        this.userService.getMatches(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(r => {
            this.matches = r.result;
            this.pagination = r.pagination;
        });
    }
    pageChanged($event) {
        if ($event) {
            this.pagination.currentPage = $event.pageIndex + 1;
            this.loadMatches();
        }
    }
}
ListsComponent.ɵfac = function ListsComponent_Factory(t) { return new (t || ListsComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_0__.UserService)); };
ListsComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({ type: ListsComponent, selectors: [["app-lists"]], decls: 2, vars: 2, consts: [["class", "container mt-4", 4, "ngIf"], ["class", "d-flex justify-content-center", 4, "ngIf"], [1, "container", "mt-4"], [1, "row", "mt-4"], ["class", "col-3", 4, "ngFor", "ngForOf"], [1, "col-3"], [3, "isLiked", "user"], [1, "d-flex", "justify-content-center"], ["aria-label", "Select page", 3, "length", "pageSize", "page"]], template: function ListsComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](0, ListsComponent_div_0_Template, 3, 1, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, ListsComponent_div_1_Template, 2, 2, "div", 1);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.matches.length > 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.pagination && ctx.pagination.totalPages > 1);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__.NgIf, _angular_common__WEBPACK_IMPORTED_MODULE_3__.NgForOf, _users_user_card_user_card_component__WEBPACK_IMPORTED_MODULE_1__.UserCardComponent, _angular_material_paginator__WEBPACK_IMPORTED_MODULE_4__.MatPaginator], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJsaXN0cy5jb21wb25lbnQuY3NzIn0= */"] });


/***/ }),

/***/ 8458:
/*!******************************************!*\
  !*** ./src/app/login/login.component.ts ***!
  \******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "LoginComponent": () => (/* binding */ LoginComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/account.service */ 7110);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../_services/user.service */ 5089);
/* harmony import */ var _angular_material_card__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/material/card */ 1961);
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/forms */ 587);
/* harmony import */ var _angular_material_form_field__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/material/form-field */ 9076);
/* harmony import */ var _angular_material_input__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/input */ 3365);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/material/button */ 7317);










function LoginComponent_mat_error_15_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-error", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, " The username and password were not recognized ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} }
class LoginComponent {
    constructor(accountService, router, userService) {
        this.accountService = accountService;
        this.router = router;
        this.userService = userService;
        this.loginValid = true;
        this.loginCred = { email: '', password: '' };
    }
    ngOnInit() {
    }
    login() {
        this.accountService.login(this.loginCred).subscribe(response => {
            if (response) {
                if (response.token) {
                    this.router.navigateByUrl('/users');
                    this.userService.deleteCachedValues();
                }
            }
        }, () => this.loginValid = false);
    }
}
LoginComponent.ɵfac = function LoginComponent_Factory(t) { return new (t || LoginComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_account_service__WEBPACK_IMPORTED_MODULE_0__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_3__.Router), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_1__.UserService)); };
LoginComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({ type: LoginComponent, selectors: [["app-login"]], decls: 18, vars: 4, consts: [[1, "row"], [3, "ngSubmit"], ["loginForm", "ngForm"], ["matInput", "", "placeholder", "Email", "name", "email", "required", "", 3, "ngModel", "ngModelChange"], ["matInput", "", "type", "password", "placeholder", "Password", "name", "password", "required", "", 3, "ngModel", "ngModelChange"], ["class", "my-1", 4, "ngIf"], ["mat-raised-button", "", "color", "primary", 3, "disabled"], [1, "my-1"]], template: function LoginComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 0)(1, "mat-card")(2, "mat-card-content")(3, "form", 1, 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngSubmit", function LoginComponent_Template_form_ngSubmit_3_listener() { return ctx.login(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](5, "h1");
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](6, "Sign In");
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](7, "mat-form-field")(8, "input", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngModelChange", function LoginComponent_Template_input_ngModelChange_8_listener($event) { return ctx.loginCred.email = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](9, "mat-error");
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](10, " Please provide a valid email address ");
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](11, "mat-form-field")(12, "input", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngModelChange", function LoginComponent_Template_input_ngModelChange_12_listener($event) { return ctx.loginCred.password = $event; });
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](13, "mat-error");
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](14, " Please provide a valid password ");
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](15, LoginComponent_mat_error_15_Template, 2, 0, "mat-error", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](16, "button", 6);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](17, "Login");
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()()()();
    } if (rf & 2) {
        const _r0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵreference"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngModel", ctx.loginCred.email);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngModel", ctx.loginCred.password);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", !ctx.loginValid);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !_r0.form.valid);
    } }, directives: [_angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCard, _angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCardContent, _angular_forms__WEBPACK_IMPORTED_MODULE_5__["ɵNgNoValidate"], _angular_forms__WEBPACK_IMPORTED_MODULE_5__.NgControlStatusGroup, _angular_forms__WEBPACK_IMPORTED_MODULE_5__.NgForm, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_6__.MatFormField, _angular_material_input__WEBPACK_IMPORTED_MODULE_7__.MatInput, _angular_forms__WEBPACK_IMPORTED_MODULE_5__.DefaultValueAccessor, _angular_forms__WEBPACK_IMPORTED_MODULE_5__.RequiredValidator, _angular_forms__WEBPACK_IMPORTED_MODULE_5__.NgControlStatus, _angular_forms__WEBPACK_IMPORTED_MODULE_5__.NgModel, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_6__.MatError, _angular_common__WEBPACK_IMPORTED_MODULE_8__.NgIf, _angular_material_button__WEBPACK_IMPORTED_MODULE_9__.MatButton], styles: ["mat-card[_ngcontent-%COMP%] {\r\n    max-width: 400px;\r\n    margin: 2em auto;\r\n    text-align: center;\r\n  }\r\n  \r\n  mat-form-field[_ngcontent-%COMP%] {\r\n    display: block;\r\n  }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbImxvZ2luLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxnQkFBZ0I7SUFDaEIsZ0JBQWdCO0lBQ2hCLGtCQUFrQjtFQUNwQjs7RUFFQTtJQUNFLGNBQWM7RUFDaEIiLCJmaWxlIjoibG9naW4uY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIm1hdC1jYXJkIHtcclxuICAgIG1heC13aWR0aDogNDAwcHg7XHJcbiAgICBtYXJnaW46IDJlbSBhdXRvO1xyXG4gICAgdGV4dC1hbGlnbjogY2VudGVyO1xyXG4gIH1cclxuICBcclxuICBtYXQtZm9ybS1maWVsZCB7XHJcbiAgICBkaXNwbGF5OiBibG9jaztcclxuICB9Il19 */"] });


/***/ }),

/***/ 5298:
/*!************************************************!*\
  !*** ./src/app/messages/messages.component.ts ***!
  \************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "MessagesComponent": () => (/* binding */ MessagesComponent)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_message_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/message.service */ 6252);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../_services/user.service */ 5089);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../_services/account.service */ 7110);
/* harmony import */ var _services_presence_service__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../_services/presence.service */ 3238);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_paginator__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/paginator */ 6439);
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/forms */ 587);
/* harmony import */ var _angular_material_form_field__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/material/form-field */ 9076);
/* harmony import */ var _angular_material_input__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/input */ 3365);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_material_icon__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @angular/material/icon */ 5590);
/* harmony import */ var ngx_timeago__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! ngx-timeago */ 2699);














const _c0 = ["sendForm"];

function MessagesComponent_li_5_div_5_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelement"](1, "span", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](2, " Active now");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }
}

function MessagesComponent_li_5_div_7_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipe"](2, "timeago");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const match_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]().$implicit;
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipeBind1"](2, 1, ctx_r5.getLoacaleDateTime(match_r3.lastActive)));
  }
}

function MessagesComponent_li_5_Template(rf, ctx) {
  if (rf & 1) {
    const _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵgetCurrentView"]();

    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "li", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵlistener"]("click", function MessagesComponent_li_5_Template_li_click_0_listener() {
      const restoredCtx = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵrestoreView"](_r8);
      const match_r3 = restoredCtx.$implicit;
      const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
      return ctx_r7.loadChat(match_r3);
    });
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelement"](1, "img", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](2, "div", 10)(3, "div", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](5, MessagesComponent_li_5_div_5_Template, 3, 0, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipe"](6, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](7, MessagesComponent_li_5_div_7_Template, 3, 3, "div", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipe"](8, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]()();
  }

  if (rf & 2) {
    const match_r3 = ctx.$implicit;
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
    let tmp_2_0;
    let tmp_3_0;
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("src", (match_r3.photos[0] == null ? null : match_r3.photos[0].url) || "./assets/Pictures/profile-placeholder.png", _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵsanitizeUrl"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtextInterpolate2"]("", match_r3.firstName, " ", match_r3.lastName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", (tmp_2_0 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipeBind1"](6, 5, ctx_r0.presenceService.onlineUsers$)) == null ? null : tmp_2_0.includes(match_r3.username));
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", !((tmp_3_0 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipeBind1"](8, 7, ctx_r0.presenceService.onlineUsers$)) == null ? null : tmp_3_0.includes(match_r3.username)));
  }
}

function MessagesComponent_div_6_Template(rf, ctx) {
  if (rf & 1) {
    const _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵgetCurrentView"]();

    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 15)(1, "mat-paginator", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵlistener"]("page", function MessagesComponent_div_6_Template_mat_paginator_page_1_listener($event) {
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵrestoreView"](_r10);
      const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
      return ctx_r9.matchPageChanged($event);
    });
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]()();
  }

  if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("length", ctx_r1.matchesPagination.totalItems)("pageSize", ctx_r1.matchesPagination.itemsPerPage);
  }
}

function MessagesComponent_div_7_small_8_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "small");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelement"](1, "span", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](2, " Active now");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }
}

function MessagesComponent_div_7_small_10_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "small");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipe"](2, "timeago");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipeBind1"](2, 1, ctx_r12.getLoacaleDateTime(ctx_r12.currentMatch.lastActive)));
  }
}

function MessagesComponent_div_7_div_12_div_1_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 34)(1, "h4", 35);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](2, "No messages yet, say hi in the message box below");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]()();
  }
}

function MessagesComponent_div_7_div_12_ul_2_li_2_div_1_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 42);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const message_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtextInterpolate1"](" ", message_r20.content, " ");
  }
}

function MessagesComponent_div_7_div_12_ul_2_li_2_div_2_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 43);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const message_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtextInterpolate1"](" ", message_r20.content, " ");
  }
}

function MessagesComponent_div_7_div_12_ul_2_li_2_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "li", 39);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](1, MessagesComponent_div_7_div_12_ul_2_li_2_div_1_Template, 2, 1, "div", 40);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](2, MessagesComponent_div_7_div_12_ul_2_li_2_div_2_Template, 2, 1, "div", 41);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const message_r20 = ctx.$implicit;
    const ctx_r19 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", message_r20.senderId == ctx_r19.currentMatch.id);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", message_r20.senderId != ctx_r19.currentMatch.id);
  }
}

function MessagesComponent_div_7_div_12_ul_2_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "ul", 36, 37);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](2, MessagesComponent_div_7_div_12_ul_2_li_2_Template, 3, 2, "li", 38);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const _r18 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵreference"](1);

    const messages_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]().$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("scrollTop", _r18.scrollHeight);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngForOf", messages_r15);
  }
}

function MessagesComponent_div_7_div_12_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 31);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](1, MessagesComponent_div_7_div_12_div_1_Template, 3, 0, "div", 32);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](2, MessagesComponent_div_7_div_12_ul_2_Template, 3, 2, "ul", 33);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const messages_r15 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", messages_r15.length === 0);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", messages_r15.length > 0);
  }
}

function MessagesComponent_div_7_Template(rf, ctx) {
  if (rf & 1) {
    const _r27 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵgetCurrentView"]();

    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 17)(1, "div", 18)(2, "div", 19)(3, "div", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelement"](4, "img", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](5, "div", 21)(6, "h3", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](8, MessagesComponent_div_7_small_8_Template, 3, 0, "small", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipe"](9, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](10, MessagesComponent_div_7_small_10_Template, 3, 3, "small", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipe"](11, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](12, MessagesComponent_div_7_div_12_Template, 3, 2, "div", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipe"](13, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](14, "form", 25, 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵlistener"]("ngSubmit", function MessagesComponent_div_7_Template_form_ngSubmit_14_listener() {
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵrestoreView"](_r27);
      const ctx_r26 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
      return ctx_r26.sendMessage();
    });
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](16, "div", 27)(17, "mat-form-field", 28)(18, "input", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵlistener"]("ngModelChange", function MessagesComponent_div_7_Template_input_ngModelChange_18_listener($event) {
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵrestoreView"](_r27);
      const ctx_r28 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
      return ctx_r28.newMessage = $event;
    });
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](19, "button", 30)(20, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtext"](21, "send");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]()()()()();
  }

  if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵnextContext"]();
    let tmp_2_0;
    let tmp_3_0;
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("src", (ctx_r2.currentMatch.photos[0] == null ? null : ctx_r2.currentMatch.photos[0].url) || "./assets/Pictures/profile-placeholder.png", _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵsanitizeUrl"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtextInterpolate2"]("", ctx_r2.currentMatch.firstName, " ", ctx_r2.currentMatch.lastName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", (tmp_2_0 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipeBind1"](9, 8, ctx_r2.presenceService.onlineUsers$)) == null ? null : tmp_2_0.includes(ctx_r2.currentMatch.username));
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", !((tmp_3_0 = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipeBind1"](11, 10, ctx_r2.presenceService.onlineUsers$)) == null ? null : tmp_3_0.includes(ctx_r2.currentMatch.username)));
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵpipeBind1"](13, 12, ctx_r2.messageService.messageThread$));
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngModel", ctx_r2.newMessage);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("disabled", ctx_r2.newMessage === "");
  }
}

class MessagesComponent {
  constructor(messageService, userService, accountService, presenceService) {
    this.messageService = messageService;
    this.userService = userService;
    this.presenceService = presenceService;
    this.mode = 'unread';
    this.matches = [];
    this.matchPageNumber = 1;
    this.matchesPerPage = 10;
    this.currentAccount = null;
    this.newMessage = "";
    accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_5__.take)(1)).subscribe(r => {
      this.currentAccount = r;
    });
  }

  getLoacaleDateTime(d) {
    var localDate = new Date(d.toString() + 'Z');
    return localDate;
  }

  ngOnInit() {
    var _a;

    this.loadMatches();

    if ((_a = history.state) === null || _a === void 0 ? void 0 : _a.username) {
      this.currentMatch = history.state;
    }

    if (this.currentMatch) {
      this.loadChat(this.currentMatch);
    }
  }

  matchPageChanged(e) {
    if (e && e.pageIndex + 1 != this.matchPageNumber) {
      this.matchPageNumber = e.pageIndex + 1;
      this.loadMatches();
    }
  }

  loadMatches() {
    this.userService.getMatches(this.matchPageNumber, this.matchesPerPage).subscribe(r => {
      if (r) {
        this.matches = r.result;
        this.matchesPagination = r.pagination;
      }
    });
  }

  sendMessage() {
    var _a;

    if (this.currentMatch) {
      this.messageService.sendMessage((_a = this.currentMatch) === null || _a === void 0 ? void 0 : _a.username, this.newMessage).then(() => {});
      this.newMessage = '';
    }
  }

  loadChat(user) {
    this.messageService.stopHubConnection();

    if (this.currentAccount) {
      this.messageService.createHubConnection(this.currentAccount, user.id);
      this.currentMatch = user;
    }
  }

  ngOnDestroy() {
    this.messageService.stopHubConnection();
  }

}

MessagesComponent.ɵfac = function MessagesComponent_Factory(t) {
  return new (t || MessagesComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdirectiveInject"](_services_message_service__WEBPACK_IMPORTED_MODULE_0__.MessageService), _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_1__.UserService), _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdirectiveInject"](_services_account_service__WEBPACK_IMPORTED_MODULE_2__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdirectiveInject"](_services_presence_service__WEBPACK_IMPORTED_MODULE_3__.PresenceService));
};

MessagesComponent.ɵcmp = /*@__PURE__*/_angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵdefineComponent"]({
  type: MessagesComponent,
  selectors: [["app-messages"]],
  viewQuery: function MessagesComponent_Query(rf, ctx) {
    if (rf & 1) {
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵviewQuery"](_c0, 5);
    }

    if (rf & 2) {
      let _t;

      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵloadQuery"]()) && (ctx.sendForm = _t.first);
    }
  },
  inputs: {
    currentMatch: "currentMatch"
  },
  decls: 8,
  vars: 3,
  consts: [[1, "row", "clearfix"], [1, "col-lg-12"], [1, "card", "chat-app"], ["id", "plist", 1, "people-list"], [1, "list-unstyled", "chat-list", "mb-0"], ["class", "clearfix", 3, "click", 4, "ngFor", "ngForOf"], ["class", "d-flex justify-content-center", 4, "ngIf"], ["class", "chat", 4, "ngIf"], [1, "clearfix", 3, "click"], ["alt", "avatar", 3, "src"], [1, "about"], [1, "name"], ["class", "status", 4, "ngIf"], [1, "status"], [1, "online"], [1, "d-flex", "justify-content-center"], ["aria-label", "Select page", 3, "length", "pageSize", "page"], [1, "chat"], [1, "chat-header", "clearfix"], [1, "row"], [1, "col-1"], [1, "col-2"], [1, "mb-0"], [4, "ngIf"], ["class", "chat-history", 4, "ngIf"], [1, "chat-message", "clearfix", 3, "ngSubmit"], ["sendForm", "ngForm"], [1, "row", 2, "height", "fit-content"], [1, "col-11"], ["matInput", "", "name", "content", "autocomplete", "off", 3, "ngModel", "ngModelChange"], ["mat-raised-button", "", "color", "primary", "type", "submit", 1, "col-1", 2, "height", "min-content", 3, "disabled"], [1, "chat-history"], ["class", "mt-4", 4, "ngIf"], ["style", "overflow-y: scroll; height: 500px;", "class", "m-b-0", 3, "scrollTop", 4, "ngIf"], [1, "mt-4"], [1, "text-center"], [1, "m-b-0", 2, "overflow-y", "scroll", "height", "500px", 3, "scrollTop"], ["scrollMe", ""], ["class", "clearfix", 4, "ngFor", "ngForOf"], [1, "clearfix"], ["class", "message my-message", 4, "ngIf"], ["class", "message other-message float-right", 4, "ngIf"], [1, "message", "my-message"], [1, "message", "other-message", "float-right"]],
  template: function MessagesComponent_Template(rf, ctx) {
    if (rf & 1) {
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementStart"](0, "div", 0)(1, "div", 1)(2, "div", 2)(3, "div", 3)(4, "ul", 4);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](5, MessagesComponent_li_5_Template, 9, 9, "li", 5);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](6, MessagesComponent_div_6_Template, 2, 2, "div", 6);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵtemplate"](7, MessagesComponent_div_7_Template, 22, 14, "div", 7);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵelementEnd"]()()();
    }

    if (rf & 2) {
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](5);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngForOf", ctx.matches);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", ctx.matchesPagination && ctx.matchesPagination.totalPages > 1);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵadvance"](1);
      _angular_core__WEBPACK_IMPORTED_MODULE_4__["ɵɵproperty"]("ngIf", ctx.currentMatch);
    }
  },
  directives: [_angular_common__WEBPACK_IMPORTED_MODULE_6__.NgForOf, _angular_common__WEBPACK_IMPORTED_MODULE_6__.NgIf, _angular_material_paginator__WEBPACK_IMPORTED_MODULE_7__.MatPaginator, _angular_forms__WEBPACK_IMPORTED_MODULE_8__["ɵNgNoValidate"], _angular_forms__WEBPACK_IMPORTED_MODULE_8__.NgControlStatusGroup, _angular_forms__WEBPACK_IMPORTED_MODULE_8__.NgForm, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_9__.MatFormField, _angular_material_input__WEBPACK_IMPORTED_MODULE_10__.MatInput, _angular_forms__WEBPACK_IMPORTED_MODULE_8__.DefaultValueAccessor, _angular_forms__WEBPACK_IMPORTED_MODULE_8__.NgControlStatus, _angular_forms__WEBPACK_IMPORTED_MODULE_8__.NgModel, _angular_material_button__WEBPACK_IMPORTED_MODULE_11__.MatButton, _angular_material_icon__WEBPACK_IMPORTED_MODULE_12__.MatIcon],
  pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_6__.AsyncPipe, ngx_timeago__WEBPACK_IMPORTED_MODULE_13__.TimeagoPipe],
  styles: [".online[_ngcontent-%COMP%]{\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: rgb(1, 189, 42);\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n}\r\n.card[_ngcontent-%COMP%] {\r\n    background: #fff;\r\n    transition: .5s;\r\n    border: 0;\r\n    margin-bottom: 30px;\r\n    border-radius: .55rem;\r\n    position: relative;\r\n    width: 100%;\r\n    box-shadow: 0 1px 2px 0 rgb(0 0 0 / 10%);\r\n}\r\n.chat-app[_ngcontent-%COMP%]   .people-list[_ngcontent-%COMP%] {\r\n    width: 280px;\r\n    position: absolute;\r\n    left: 0;\r\n    top: 0;\r\n    padding: 20px;\r\n    z-index: 7\r\n}\r\n.chat-app[_ngcontent-%COMP%]   .chat[_ngcontent-%COMP%] {\r\n    margin-left: 280px;\r\n    border-left: 1px solid #eaeaea\r\n}\r\n.people-list[_ngcontent-%COMP%] {\r\n    transition: .5s\r\n}\r\n.people-list[_ngcontent-%COMP%]   .chat-list[_ngcontent-%COMP%]   li[_ngcontent-%COMP%] {\r\n    padding: 10px 15px;\r\n    list-style: none;\r\n    border-radius: 3px\r\n}\r\n.people-list[_ngcontent-%COMP%]   .chat-list[_ngcontent-%COMP%]   li[_ngcontent-%COMP%]:hover {\r\n    background: #efefef;\r\n    cursor: pointer\r\n}\r\n.people-list[_ngcontent-%COMP%]   .chat-list[_ngcontent-%COMP%]   li.active[_ngcontent-%COMP%] {\r\n    background: #efefef\r\n}\r\n.people-list[_ngcontent-%COMP%]   .chat-list[_ngcontent-%COMP%]   li[_ngcontent-%COMP%]   .name[_ngcontent-%COMP%] {\r\n    font-size: 15px\r\n}\r\n.people-list[_ngcontent-%COMP%]   .chat-list[_ngcontent-%COMP%]   img[_ngcontent-%COMP%] {\r\n    width: 45px;\r\n    border-radius: 50%\r\n}\r\n.people-list[_ngcontent-%COMP%]   img[_ngcontent-%COMP%] {\r\n    float: left;\r\n    border-radius: 50%\r\n}\r\n.people-list[_ngcontent-%COMP%]   .about[_ngcontent-%COMP%] {\r\n    float: left;\r\n    padding-left: 8px\r\n}\r\n.people-list[_ngcontent-%COMP%]   .status[_ngcontent-%COMP%] {\r\n    color: #999;\r\n    font-size: 13px\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-header[_ngcontent-%COMP%] {\r\n    padding: 15px 20px;\r\n    border-bottom: 2px solid #f4f7f6\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-header[_ngcontent-%COMP%]   img[_ngcontent-%COMP%] {\r\n    float: left;\r\n    border-radius: 40px;\r\n    width: 40px\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%] {\r\n    padding: 16px;\r\n    border-bottom: 2px solid #fff\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%]   ul[_ngcontent-%COMP%] {\r\n    padding: 0\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%]   ul[_ngcontent-%COMP%]   li[_ngcontent-%COMP%] {\r\n    list-style: none;\r\n    margin-bottom: 8px\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%]   .message-data[_ngcontent-%COMP%] {\r\n    margin-bottom: 8px\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%]   .message[_ngcontent-%COMP%] {\r\n    color: #444;\r\n    padding: 8px;\r\n    line-height: 26px;\r\n    font-size: 16px;\r\n    border-radius: 7px;\r\n    display: inline-block;\r\n    position: relative\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%]   .my-message[_ngcontent-%COMP%] {\r\n    background: #f3f3f3\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%]   .other-message[_ngcontent-%COMP%] {\r\n    background: #D2D2FF;\r\n    text-align: right;\r\n    margin-right: 8px;\r\n}\r\n.chat[_ngcontent-%COMP%]   .chat-message[_ngcontent-%COMP%] {\r\n    padding: 20px\r\n}\r\n.float-right[_ngcontent-%COMP%] {\r\n    float: right\r\n}\r\n.clearfix[_ngcontent-%COMP%]:after {\r\n    visibility: hidden;\r\n    display: block;\r\n    font-size: 0;\r\n    content: \" \";\r\n    clear: both;\r\n    height: 0\r\n}\r\n@media only screen and (max-width: 767px) {\r\n    .chat-app[_ngcontent-%COMP%]   .people-list[_ngcontent-%COMP%] {\r\n        height: 465px;\r\n        width: 100%;\r\n        overflow-x: auto;\r\n        background: #fff;\r\n        left: -400px;\r\n        display: none\r\n    }\r\n    .chat-app[_ngcontent-%COMP%]   .people-list.open[_ngcontent-%COMP%] {\r\n        left: 0\r\n    }\r\n    .chat-app[_ngcontent-%COMP%]   .chat[_ngcontent-%COMP%] {\r\n        margin: 0\r\n    }\r\n    .chat-app[_ngcontent-%COMP%]   .chat[_ngcontent-%COMP%]   .chat-header[_ngcontent-%COMP%] {\r\n        border-radius: 0.55rem 0.55rem 0 0\r\n    }\r\n    .chat-app[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%] {\r\n        height: 300px;\r\n        overflow-x: auto\r\n    }\r\n}\r\n@media only screen and (min-width: 768px) and (max-width: 992px) {\r\n    .chat-app[_ngcontent-%COMP%]   .chat-list[_ngcontent-%COMP%] {\r\n        height: 650px;\r\n        overflow-x: auto\r\n    }\r\n    .chat-app[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%] {\r\n        height: 600px;\r\n        overflow-x: auto\r\n    }\r\n}\r\n@media only screen and (min-device-width: 768px) and (max-device-width: 1024px) and (orientation: landscape) and (-webkit-min-device-pixel-ratio: 1) {\r\n    .chat-app[_ngcontent-%COMP%]   .chat-list[_ngcontent-%COMP%] {\r\n        height: 480px;\r\n        overflow-x: auto\r\n    }\r\n    .chat-app[_ngcontent-%COMP%]   .chat-history[_ngcontent-%COMP%] {\r\n        height: calc(100vh - 350px);\r\n        overflow-x: auto\r\n    }\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm1lc3NhZ2VzLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxZQUFZO0lBQ1osV0FBVztJQUNYLGlDQUFpQztJQUNqQyxrQkFBa0I7SUFDbEIscUJBQXFCO0FBQ3pCO0FBQ0E7SUFDSSxnQkFBZ0I7SUFDaEIsZUFBZTtJQUNmLFNBQVM7SUFDVCxtQkFBbUI7SUFDbkIscUJBQXFCO0lBQ3JCLGtCQUFrQjtJQUNsQixXQUFXO0lBQ1gsd0NBQXdDO0FBQzVDO0FBQ0E7SUFDSSxZQUFZO0lBQ1osa0JBQWtCO0lBQ2xCLE9BQU87SUFDUCxNQUFNO0lBQ04sYUFBYTtJQUNiO0FBQ0o7QUFFQTtJQUNJLGtCQUFrQjtJQUNsQjtBQUNKO0FBRUE7SUFJSTtBQUNKO0FBRUE7SUFDSSxrQkFBa0I7SUFDbEIsZ0JBQWdCO0lBQ2hCO0FBQ0o7QUFFQTtJQUNJLG1CQUFtQjtJQUNuQjtBQUNKO0FBRUE7SUFDSTtBQUNKO0FBRUE7SUFDSTtBQUNKO0FBRUE7SUFDSSxXQUFXO0lBQ1g7QUFDSjtBQUVBO0lBQ0ksV0FBVztJQUNYO0FBQ0o7QUFFQTtJQUNJLFdBQVc7SUFDWDtBQUNKO0FBRUE7SUFDSSxXQUFXO0lBQ1g7QUFDSjtBQUVBO0lBQ0ksa0JBQWtCO0lBQ2xCO0FBQ0o7QUFFQTtJQUNJLFdBQVc7SUFDWCxtQkFBbUI7SUFDbkI7QUFDSjtBQUNBO0lBQ0ksYUFBYTtJQUNiO0FBQ0o7QUFFQTtJQUNJO0FBQ0o7QUFFQTtJQUNJLGdCQUFnQjtJQUNoQjtBQUNKO0FBQ0E7SUFDSTtBQUNKO0FBQ0E7SUFDSSxXQUFXO0lBQ1gsWUFBWTtJQUNaLGlCQUFpQjtJQUNqQixlQUFlO0lBQ2Ysa0JBQWtCO0lBQ2xCLHFCQUFxQjtJQUNyQjtBQUNKO0FBRUE7SUFDSTtBQUNKO0FBRUE7SUFDSSxtQkFBbUI7SUFDbkIsaUJBQWlCO0lBQ2pCLGlCQUFpQjtBQUNyQjtBQUNBO0lBQ0k7QUFDSjtBQUVBO0lBQ0k7QUFDSjtBQUVBO0lBQ0ksa0JBQWtCO0lBQ2xCLGNBQWM7SUFDZCxZQUFZO0lBQ1osWUFBWTtJQUNaLFdBQVc7SUFDWDtBQUNKO0FBRUE7SUFDSTtRQUNJLGFBQWE7UUFDYixXQUFXO1FBQ1gsZ0JBQWdCO1FBQ2hCLGdCQUFnQjtRQUNoQixZQUFZO1FBQ1o7SUFDSjtJQUNBO1FBQ0k7SUFDSjtJQUNBO1FBQ0k7SUFDSjtJQUNBO1FBQ0k7SUFDSjtJQUNBO1FBQ0ksYUFBYTtRQUNiO0lBQ0o7QUFDSjtBQUVBO0lBQ0k7UUFDSSxhQUFhO1FBQ2I7SUFDSjtJQUNBO1FBQ0ksYUFBYTtRQUNiO0lBQ0o7QUFDSjtBQUVBO0lBQ0k7UUFDSSxhQUFhO1FBQ2I7SUFDSjtJQUNBO1FBQ0ksMkJBQTJCO1FBQzNCO0lBQ0o7QUFDSiIsImZpbGUiOiJtZXNzYWdlcy5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLm9ubGluZXtcclxuICAgIGhlaWdodDogMTBweDtcclxuICAgIHdpZHRoOiAxMHB4O1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogcmdiKDEsIDE4OSwgNDIpO1xyXG4gICAgYm9yZGVyLXJhZGl1czogNTAlO1xyXG4gICAgZGlzcGxheTogaW5saW5lLWJsb2NrO1xyXG59XHJcbi5jYXJkIHtcclxuICAgIGJhY2tncm91bmQ6ICNmZmY7XHJcbiAgICB0cmFuc2l0aW9uOiAuNXM7XHJcbiAgICBib3JkZXI6IDA7XHJcbiAgICBtYXJnaW4tYm90dG9tOiAzMHB4O1xyXG4gICAgYm9yZGVyLXJhZGl1czogLjU1cmVtO1xyXG4gICAgcG9zaXRpb246IHJlbGF0aXZlO1xyXG4gICAgd2lkdGg6IDEwMCU7XHJcbiAgICBib3gtc2hhZG93OiAwIDFweCAycHggMCByZ2IoMCAwIDAgLyAxMCUpO1xyXG59XHJcbi5jaGF0LWFwcCAucGVvcGxlLWxpc3Qge1xyXG4gICAgd2lkdGg6IDI4MHB4O1xyXG4gICAgcG9zaXRpb246IGFic29sdXRlO1xyXG4gICAgbGVmdDogMDtcclxuICAgIHRvcDogMDtcclxuICAgIHBhZGRpbmc6IDIwcHg7XHJcbiAgICB6LWluZGV4OiA3XHJcbn1cclxuXHJcbi5jaGF0LWFwcCAuY2hhdCB7XHJcbiAgICBtYXJnaW4tbGVmdDogMjgwcHg7XHJcbiAgICBib3JkZXItbGVmdDogMXB4IHNvbGlkICNlYWVhZWFcclxufVxyXG5cclxuLnBlb3BsZS1saXN0IHtcclxuICAgIC1tb3otdHJhbnNpdGlvbjogLjVzO1xyXG4gICAgLW8tdHJhbnNpdGlvbjogLjVzO1xyXG4gICAgLXdlYmtpdC10cmFuc2l0aW9uOiAuNXM7XHJcbiAgICB0cmFuc2l0aW9uOiAuNXNcclxufVxyXG5cclxuLnBlb3BsZS1saXN0IC5jaGF0LWxpc3QgbGkge1xyXG4gICAgcGFkZGluZzogMTBweCAxNXB4O1xyXG4gICAgbGlzdC1zdHlsZTogbm9uZTtcclxuICAgIGJvcmRlci1yYWRpdXM6IDNweFxyXG59XHJcblxyXG4ucGVvcGxlLWxpc3QgLmNoYXQtbGlzdCBsaTpob3ZlciB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZWZlZmVmO1xyXG4gICAgY3Vyc29yOiBwb2ludGVyXHJcbn1cclxuXHJcbi5wZW9wbGUtbGlzdCAuY2hhdC1saXN0IGxpLmFjdGl2ZSB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjZWZlZmVmXHJcbn1cclxuXHJcbi5wZW9wbGUtbGlzdCAuY2hhdC1saXN0IGxpIC5uYW1lIHtcclxuICAgIGZvbnQtc2l6ZTogMTVweFxyXG59XHJcblxyXG4ucGVvcGxlLWxpc3QgLmNoYXQtbGlzdCBpbWcge1xyXG4gICAgd2lkdGg6IDQ1cHg7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCVcclxufVxyXG5cclxuLnBlb3BsZS1saXN0IGltZyB7XHJcbiAgICBmbG9hdDogbGVmdDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJVxyXG59XHJcblxyXG4ucGVvcGxlLWxpc3QgLmFib3V0IHtcclxuICAgIGZsb2F0OiBsZWZ0O1xyXG4gICAgcGFkZGluZy1sZWZ0OiA4cHhcclxufVxyXG5cclxuLnBlb3BsZS1saXN0IC5zdGF0dXMge1xyXG4gICAgY29sb3I6ICM5OTk7XHJcbiAgICBmb250LXNpemU6IDEzcHhcclxufVxyXG5cclxuLmNoYXQgLmNoYXQtaGVhZGVyIHtcclxuICAgIHBhZGRpbmc6IDE1cHggMjBweDtcclxuICAgIGJvcmRlci1ib3R0b206IDJweCBzb2xpZCAjZjRmN2Y2XHJcbn1cclxuXHJcbi5jaGF0IC5jaGF0LWhlYWRlciBpbWcge1xyXG4gICAgZmxvYXQ6IGxlZnQ7XHJcbiAgICBib3JkZXItcmFkaXVzOiA0MHB4O1xyXG4gICAgd2lkdGg6IDQwcHhcclxufVxyXG4uY2hhdCAuY2hhdC1oaXN0b3J5IHtcclxuICAgIHBhZGRpbmc6IDE2cHg7XHJcbiAgICBib3JkZXItYm90dG9tOiAycHggc29saWQgI2ZmZlxyXG59XHJcblxyXG4uY2hhdCAuY2hhdC1oaXN0b3J5IHVsIHtcclxuICAgIHBhZGRpbmc6IDBcclxufVxyXG5cclxuLmNoYXQgLmNoYXQtaGlzdG9yeSB1bCBsaSB7XHJcbiAgICBsaXN0LXN0eWxlOiBub25lO1xyXG4gICAgbWFyZ2luLWJvdHRvbTogOHB4XHJcbn1cclxuLmNoYXQgLmNoYXQtaGlzdG9yeSAubWVzc2FnZS1kYXRhIHtcclxuICAgIG1hcmdpbi1ib3R0b206IDhweFxyXG59XHJcbi5jaGF0IC5jaGF0LWhpc3RvcnkgLm1lc3NhZ2Uge1xyXG4gICAgY29sb3I6ICM0NDQ7XHJcbiAgICBwYWRkaW5nOiA4cHg7XHJcbiAgICBsaW5lLWhlaWdodDogMjZweDtcclxuICAgIGZvbnQtc2l6ZTogMTZweDtcclxuICAgIGJvcmRlci1yYWRpdXM6IDdweDtcclxuICAgIGRpc3BsYXk6IGlubGluZS1ibG9jaztcclxuICAgIHBvc2l0aW9uOiByZWxhdGl2ZVxyXG59XHJcblxyXG4uY2hhdCAuY2hhdC1oaXN0b3J5IC5teS1tZXNzYWdlIHtcclxuICAgIGJhY2tncm91bmQ6ICNmM2YzZjNcclxufVxyXG5cclxuLmNoYXQgLmNoYXQtaGlzdG9yeSAub3RoZXItbWVzc2FnZSB7XHJcbiAgICBiYWNrZ3JvdW5kOiAjRDJEMkZGO1xyXG4gICAgdGV4dC1hbGlnbjogcmlnaHQ7XHJcbiAgICBtYXJnaW4tcmlnaHQ6IDhweDtcclxufVxyXG4uY2hhdCAuY2hhdC1tZXNzYWdlIHtcclxuICAgIHBhZGRpbmc6IDIwcHhcclxufVxyXG5cclxuLmZsb2F0LXJpZ2h0IHtcclxuICAgIGZsb2F0OiByaWdodFxyXG59XHJcblxyXG4uY2xlYXJmaXg6YWZ0ZXIge1xyXG4gICAgdmlzaWJpbGl0eTogaGlkZGVuO1xyXG4gICAgZGlzcGxheTogYmxvY2s7XHJcbiAgICBmb250LXNpemU6IDA7XHJcbiAgICBjb250ZW50OiBcIiBcIjtcclxuICAgIGNsZWFyOiBib3RoO1xyXG4gICAgaGVpZ2h0OiAwXHJcbn1cclxuXHJcbkBtZWRpYSBvbmx5IHNjcmVlbiBhbmQgKG1heC13aWR0aDogNzY3cHgpIHtcclxuICAgIC5jaGF0LWFwcCAucGVvcGxlLWxpc3Qge1xyXG4gICAgICAgIGhlaWdodDogNDY1cHg7XHJcbiAgICAgICAgd2lkdGg6IDEwMCU7XHJcbiAgICAgICAgb3ZlcmZsb3cteDogYXV0bztcclxuICAgICAgICBiYWNrZ3JvdW5kOiAjZmZmO1xyXG4gICAgICAgIGxlZnQ6IC00MDBweDtcclxuICAgICAgICBkaXNwbGF5OiBub25lXHJcbiAgICB9XHJcbiAgICAuY2hhdC1hcHAgLnBlb3BsZS1saXN0Lm9wZW4ge1xyXG4gICAgICAgIGxlZnQ6IDBcclxuICAgIH1cclxuICAgIC5jaGF0LWFwcCAuY2hhdCB7XHJcbiAgICAgICAgbWFyZ2luOiAwXHJcbiAgICB9XHJcbiAgICAuY2hhdC1hcHAgLmNoYXQgLmNoYXQtaGVhZGVyIHtcclxuICAgICAgICBib3JkZXItcmFkaXVzOiAwLjU1cmVtIDAuNTVyZW0gMCAwXHJcbiAgICB9XHJcbiAgICAuY2hhdC1hcHAgLmNoYXQtaGlzdG9yeSB7XHJcbiAgICAgICAgaGVpZ2h0OiAzMDBweDtcclxuICAgICAgICBvdmVyZmxvdy14OiBhdXRvXHJcbiAgICB9XHJcbn1cclxuXHJcbkBtZWRpYSBvbmx5IHNjcmVlbiBhbmQgKG1pbi13aWR0aDogNzY4cHgpIGFuZCAobWF4LXdpZHRoOiA5OTJweCkge1xyXG4gICAgLmNoYXQtYXBwIC5jaGF0LWxpc3Qge1xyXG4gICAgICAgIGhlaWdodDogNjUwcHg7XHJcbiAgICAgICAgb3ZlcmZsb3cteDogYXV0b1xyXG4gICAgfVxyXG4gICAgLmNoYXQtYXBwIC5jaGF0LWhpc3Rvcnkge1xyXG4gICAgICAgIGhlaWdodDogNjAwcHg7XHJcbiAgICAgICAgb3ZlcmZsb3cteDogYXV0b1xyXG4gICAgfVxyXG59XHJcblxyXG5AbWVkaWEgb25seSBzY3JlZW4gYW5kIChtaW4tZGV2aWNlLXdpZHRoOiA3NjhweCkgYW5kIChtYXgtZGV2aWNlLXdpZHRoOiAxMDI0cHgpIGFuZCAob3JpZW50YXRpb246IGxhbmRzY2FwZSkgYW5kICgtd2Via2l0LW1pbi1kZXZpY2UtcGl4ZWwtcmF0aW86IDEpIHtcclxuICAgIC5jaGF0LWFwcCAuY2hhdC1saXN0IHtcclxuICAgICAgICBoZWlnaHQ6IDQ4MHB4O1xyXG4gICAgICAgIG92ZXJmbG93LXg6IGF1dG9cclxuICAgIH1cclxuICAgIC5jaGF0LWFwcCAuY2hhdC1oaXN0b3J5IHtcclxuICAgICAgICBoZWlnaHQ6IGNhbGMoMTAwdmggLSAzNTBweCk7XHJcbiAgICAgICAgb3ZlcmZsb3cteDogYXV0b1xyXG4gICAgfVxyXG59Il19 */"],
  changeDetection: 0
});

/***/ }),

/***/ 5375:
/*!**************************************!*\
  !*** ./src/app/nav/nav.component.ts ***!
  \**************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "NavComponent": () => (/* binding */ NavComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/user.service */ 5089);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../_services/account.service */ 7110);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ngx-toastr */ 4101);
/* harmony import */ var _angular_material_toolbar__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/material/toolbar */ 9946);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _directives_has_role_directive__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../_directives/has-role.directive */ 397);
/* harmony import */ var _angular_material_menu__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/material/menu */ 2796);
/* harmony import */ var _angular_material_icon__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/icon */ 5590);












function NavComponent_ng_container_5_button_7_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "button", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](1, "Admin");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
  }
}

const _c0 = function () {
  return ["admin", "moderator"];
};

function NavComponent_ng_container_5_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementContainerStart"](0);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](1, "button", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](2, "Explore");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](3, "button", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](4, "Matches");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](5, "button", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](6, "Messages");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](7, NavComponent_ng_container_5_button_7_Template, 2, 0, "button", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementContainerEnd"]();
  }

  if (rf & 2) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("appHasRole", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpureFunction0"](1, _c0));
  }
}

function NavComponent_div_7_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "div", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](1, "img", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](2, "div", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](4, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](5, "arrow_drop_down");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
  }

  if (rf & 2) {
    const user_r5 = ctx.ngIf;
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"]();

    const _r2 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵreference"](10);

    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("matMenuTriggerFor", _r2);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpropertyInterpolate"]("src", (user_r5.userData.photos[0] == null ? null : user_r5.userData.photos[0].url) || "./assets/Pictures/profile-placeholder.png", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵsanitizeUrl"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpropertyInterpolate"]("alt", user_r5.userData.firstName);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate2"](" ", user_r5.userData.firstName, " ", user_r5.userData.lastName, " ");
  }
}

function NavComponent_button_19_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "button", 17)(1, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](2, "login");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](3, " Sign in ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
  }
}

class NavComponent {
  constructor(userService, accountService, router, toastr) {
    this.userService = userService;
    this.accountService = accountService;
    this.router = router;
    this.toastr = toastr;
  }

  ngOnInit() {}

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }

  route() {
    let userString = localStorage.getItem('user');

    if (userString) {
      let user = JSON.parse(userString);

      if (user.token) {
        this.router.navigateByUrl('/users');
        return;
      }
    }

    this.router.navigateByUrl('/home');
  }

}

NavComponent.ɵfac = function NavComponent_Factory(t) {
  return new (t || NavComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_0__.UserService), _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](_services_account_service__WEBPACK_IMPORTED_MODULE_1__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_4__.Router), _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_5__.ToastrService));
};

NavComponent.ɵcmp = /*@__PURE__*/_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdefineComponent"]({
  type: NavComponent,
  selectors: [["app-nav"]],
  decls: 21,
  vars: 9,
  consts: [["color", "primary", 1, "d-flex", "flex-row", "align-items-center", "p-2", 2, "height", "fit-content", "box-shadow", "0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19)"], ["mat-button", "", 2, "font-size", "large", 3, "click"], [1, "me-auto", 2, "font-size", "medium"], [4, "ngIf"], ["class", "d-flex pointer align-items-center", 3, "matMenuTriggerFor", 4, "ngIf"], ["menu", "matMenu"], ["mat-menu-item", "", "routerLink", "users/edit"], ["mat-menu-item", "", 3, "click"], ["mat-button", "", "routerLink", "/login", 4, "ngIf"], ["mat-flat-button", "", "color", "primary", "routerLink", "/users", "routerActiveLink", "active"], ["mat-flat-button", "", "color", "primary", "routerLink", "/lists", "routerActiveLink", "active"], ["mat-flat-button", "", "color", "primary", "routerLink", "/messages", "routerActiveLink", "active"], ["mat-flat-button", "", "color", "primary", "routerLink", "/admin", "routerActiveLink", "active", 4, "appHasRole"], ["mat-flat-button", "", "color", "primary", "routerLink", "/admin", "routerActiveLink", "active"], [1, "d-flex", "pointer", "align-items-center", 3, "matMenuTriggerFor"], [1, "nav-img-circle", "me-2", 3, "src", "alt"], [1, "ms-1", 2, "font-size", "medium"], ["mat-button", "", "routerLink", "/login"]],
  template: function NavComponent_Template(rf, ctx) {
    if (rf & 1) {
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "mat-toolbar", 0)(1, "div")(2, "button", 1);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function NavComponent_Template_button_click_2_listener() {
        return ctx.route();
      });
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](3, "Dating App");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](4, "div", 2);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](5, NavComponent_ng_container_5_Template, 8, 2, "ng-container", 3);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipe"](6, "async");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](7, NavComponent_div_7_Template, 6, 5, "div", 4);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipe"](8, "async");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](9, "mat-menu", null, 5)(11, "button", 6)(12, "mat-icon");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](13, "edit");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](14, " Edit Profile ");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](15, "button", 7);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function NavComponent_Template_button_click_15_listener() {
        return ctx.logout();
      });
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](16, "mat-icon");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](17, "logout");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](18, " Logout ");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](19, NavComponent_button_19_Template, 4, 0, "button", 8);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipe"](20, "async");
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    }

    if (rf & 2) {
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](5);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipeBind1"](6, 3, ctx.accountService.currentUser$));
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](2);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipeBind1"](8, 5, ctx.accountService.currentUser$));
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](12);
      _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipeBind1"](20, 7, ctx.accountService.currentUser$) === null);
    }
  },
  directives: [_angular_material_toolbar__WEBPACK_IMPORTED_MODULE_6__.MatToolbar, _angular_material_button__WEBPACK_IMPORTED_MODULE_7__.MatButton, _angular_common__WEBPACK_IMPORTED_MODULE_8__.NgIf, _angular_router__WEBPACK_IMPORTED_MODULE_4__.RouterLink, _directives_has_role_directive__WEBPACK_IMPORTED_MODULE_2__.HasRoleDirective, _angular_material_menu__WEBPACK_IMPORTED_MODULE_9__.MatMenuTrigger, _angular_material_icon__WEBPACK_IMPORTED_MODULE_10__.MatIcon, _angular_material_menu__WEBPACK_IMPORTED_MODULE_9__.MatMenu, _angular_material_menu__WEBPACK_IMPORTED_MODULE_9__.MatMenuItem],
  pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_8__.AsyncPipe],
  styles: [".pointer[_ngcontent-%COMP%]{\r\n    cursor:pointer;\r\n    text-decoration: none;\r\n    -webkit-text-size-adjust: 10pt;\r\n            text-size-adjust: 10pt;\r\n}\r\n.nav-img-circle[_ngcontent-%COMP%] {\r\n    max-height: 40px;\r\n    max-width: 40px;\r\n\r\n    border-radius: 50%;\r\n    display: inline;\r\n}\r\n.example-spacer[_ngcontent-%COMP%] {\r\n    flex: 1 1 auto;\r\n  }\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIm5hdi5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksY0FBYztJQUNkLHFCQUFxQjtJQUNyQiw4QkFBc0I7WUFBdEIsc0JBQXNCO0FBQzFCO0FBQ0E7SUFDSSxnQkFBZ0I7SUFDaEIsZUFBZTs7SUFFZixrQkFBa0I7SUFDbEIsZUFBZTtBQUNuQjtBQUNBO0lBQ0ksY0FBYztFQUNoQiIsImZpbGUiOiJuYXYuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5wb2ludGVye1xyXG4gICAgY3Vyc29yOnBvaW50ZXI7XHJcbiAgICB0ZXh0LWRlY29yYXRpb246IG5vbmU7XHJcbiAgICB0ZXh0LXNpemUtYWRqdXN0OiAxMHB0O1xyXG59XHJcbi5uYXYtaW1nLWNpcmNsZSB7XHJcbiAgICBtYXgtaGVpZ2h0OiA0MHB4O1xyXG4gICAgbWF4LXdpZHRoOiA0MHB4O1xyXG5cclxuICAgIGJvcmRlci1yYWRpdXM6IDUwJTtcclxuICAgIGRpc3BsYXk6IGlubGluZTtcclxufVxyXG4uZXhhbXBsZS1zcGFjZXIge1xyXG4gICAgZmxleDogMSAxIGF1dG87XHJcbiAgfSJdfQ== */"]
});

/***/ }),

/***/ 9087:
/*!************************************************!*\
  !*** ./src/app/register/register.component.ts ***!
  \************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "RegisterComponent": () => (/* binding */ RegisterComponent)
/* harmony export */ });
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/forms */ 587);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../_services/account.service */ 7110);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ngx-toastr */ 4101);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../_services/user.service */ 5089);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_form_field__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/form-field */ 9076);
/* harmony import */ var _angular_material_input__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/material/input */ 3365);
/* harmony import */ var _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/material/datepicker */ 5818);
/* harmony import */ var _angular_material_select__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/select */ 1434);
/* harmony import */ var _angular_material_core__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/material/core */ 8133);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @angular/material/button */ 7317);














function RegisterComponent_form_3_mat_option_62_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-option", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const country_r3 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpropertyInterpolate"]("value", country_r3);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](country_r3);
} }
function RegisterComponent_form_3_Template(rf, ctx) { if (rf & 1) {
    const _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "form", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngSubmit", function RegisterComponent_form_3_Template_form_ngSubmit_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r5); const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r4.registerForm.valid && ctx_r4.register(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](1, "h2", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, "Create your account");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](3, "hr");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](4, "div", 6)(5, "mat-form-field", 7)(6, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](7, "First name");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](8, "input", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](9, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](10, " Invalid First Name ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](11, "mat-form-field", 7)(12, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](13, "Last Name");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](14, "input", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](15, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](16, " Invalid Last Name ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](17, "div", 6)(18, "mat-form-field", 10)(19, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](20, " Username ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](21, "input", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](22, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](23, " Invalid Username ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](24, "div", 6)(25, "mat-form-field", 10)(26, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](27, "Choose a date");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](28, "input", 12)(29, "mat-datepicker-toggle", 13)(30, "mat-datepicker", null, 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](32, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](33, " Invalid Date Of Birth ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](34, "div", 6)(35, "mat-form-field", 7)(36, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](37, "Sex");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](38, "mat-select", 15)(39, "mat-option", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](40, "Male");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](41, "mat-option", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](42, "Female");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](43, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](44, " Invalid Field ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](45, "mat-form-field", 7)(46, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](47, "Interested In");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](48, "mat-select", 18)(49, "mat-option", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](50, "Males");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](51, "mat-option", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](52, "Females");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](53, "mat-option", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](54, "Both");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](55, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](56, " Invalid Field ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](57, "div", 6)(58, "mat-form-field", 7)(59, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](60, "Country");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](61, "mat-select", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](62, RegisterComponent_form_3_mat_option_62_Template, 2, 2, "mat-option", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](63, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](64, " Invalid Country ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](65, "mat-form-field", 7)(66, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](67, "City");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](68, "input", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](69, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](70, " Invalid City ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](71, "div", 6)(72, "mat-form-field", 10)(73, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](74, " Email ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](75, "input", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](76, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](77, " Invalid Email ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](78, "div", 6)(79, "mat-form-field", 10)(80, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](81, " Password ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](82, "input", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](83, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](84, " Invalid Password ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](85, "div", 6)(86, "mat-form-field", 10)(87, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](88, " Confirm password ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](89, "input", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](90, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](91, " Invalid field ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](92, "div", 26)(93, "span");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](94, " By clicking Sign up, you agree to our ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](95, "a", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](96, "Terms of Use");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](97, " and our ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](98, "a", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](99, "Privacy Policy");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](100, ". ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](101, "div", 28)(102, "button", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](103, "Sign up");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
} if (rf & 2) {
    const _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵreference"](31);
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("formGroup", ctx_r0.registerForm);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](28);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("max", ctx_r0.maxDate)("matDatepicker", _r1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("for", _r1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](33);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngForOf", ctx_r0.countryList);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](40);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("disabled", !ctx_r0.registerForm.valid);
} }
class RegisterComponent {
    constructor(accountService, toastr, formBuilder, router, userService) {
        this.accountService = accountService;
        this.toastr = toastr;
        this.formBuilder = formBuilder;
        this.router = router;
        this.userService = userService;
        this.registerForm = this.formBuilder.group({
            username: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            firstName: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            lastName: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            dateOfBirth: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            country: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            city: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            email: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            sex: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            interest: ['', _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required],
            password: ['', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.minLength(6)]],
            confirmPassword: ['', [_angular_forms__WEBPACK_IMPORTED_MODULE_3__.Validators.required, this.passwordConfirming]]
        });
        this.maxDate = new Date();
        this.countryList = [
            "Afghanistan",
            "Albania",
            "Algeria",
            "American Samoa",
            "Andorra",
            "Angola",
            "Anguilla",
            "Antarctica",
            "Antigua and Barbuda",
            "Argentina",
            "Armenia",
            "Aruba",
            "Australia",
            "Austria",
            "Azerbaijan",
            "Bahamas (the)",
            "Bahrain",
            "Bangladesh",
            "Barbados",
            "Belarus",
            "Belgium",
            "Belize",
            "Benin",
            "Bermuda",
            "Bhutan",
            "Bolivia (Plurinational State of)",
            "Bonaire, Sint Eustatius and Saba",
            "Bosnia and Herzegovina",
            "Botswana",
            "Bouvet Island",
            "Brazil",
            "British Indian Ocean Territory (the)",
            "Brunei Darussalam",
            "Bulgaria",
            "Burkina Faso",
            "Burundi",
            "Cabo Verde",
            "Cambodia",
            "Cameroon",
            "Canada",
            "Cayman Islands (the)",
            "Central African Republic (the)",
            "Chad",
            "Chile",
            "China",
            "Christmas Island",
            "Cocos (Keeling) Islands (the)",
            "Colombia",
            "Comoros (the)",
            "Congo (the Democratic Republic of the)",
            "Congo (the)",
            "Cook Islands (the)",
            "Costa Rica",
            "Croatia",
            "Cuba",
            "Curaçao",
            "Cyprus",
            "Czechia",
            "Côte d'Ivoire",
            "Denmark",
            "Djibouti",
            "Dominica",
            "Dominican Republic (the)",
            "Ecuador",
            "Egypt",
            "El Salvador",
            "Equatorial Guinea",
            "Eritrea",
            "Estonia",
            "Eswatini",
            "Ethiopia",
            "Falkland Islands (the) [Malvinas]",
            "Faroe Islands (the)",
            "Fiji",
            "Finland",
            "France",
            "French Guiana",
            "French Polynesia",
            "French Southern Territories (the)",
            "Gabon",
            "Gambia (the)",
            "Georgia",
            "Germany",
            "Ghana",
            "Gibraltar",
            "Greece",
            "Greenland",
            "Grenada",
            "Guadeloupe",
            "Guam",
            "Guatemala",
            "Guernsey",
            "Guinea",
            "Guinea-Bissau",
            "Guyana",
            "Haiti",
            "Heard Island and McDonald Islands",
            "Holy See (the)",
            "Honduras",
            "Hong Kong",
            "Hungary",
            "Iceland",
            "India",
            "Indonesia",
            "Iran (Islamic Republic of)",
            "Iraq",
            "Ireland",
            "Isle of Man",
            "Israel",
            "Italy",
            "Jamaica",
            "Japan",
            "Jersey",
            "Jordan",
            "Kazakhstan",
            "Kenya",
            "Kiribati",
            "Korea (the Democratic People's Republic of)",
            "Korea (the Republic of)",
            "Kuwait",
            "Kyrgyzstan",
            "Lao People's Democratic Republic (the)",
            "Latvia",
            "Lebanon",
            "Lesotho",
            "Liberia",
            "Libya",
            "Liechtenstein",
            "Lithuania",
            "Luxembourg",
            "Macao",
            "Madagascar",
            "Malawi",
            "Malaysia",
            "Maldives",
            "Mali",
            "Malta",
            "Marshall Islands (the)",
            "Martinique",
            "Mauritania",
            "Mauritius",
            "Mayotte",
            "Mexico",
            "Micronesia (Federated States of)",
            "Moldova (the Republic of)",
            "Monaco",
            "Mongolia",
            "Montenegro",
            "Montserrat",
            "Morocco",
            "Mozambique",
            "Myanmar",
            "Namibia",
            "Nauru",
            "Nepal",
            "Netherlands (the)",
            "New Caledonia",
            "New Zealand",
            "Nicaragua",
            "Niger (the)",
            "Nigeria",
            "Niue",
            "Norfolk Island",
            "Northern Mariana Islands (the)",
            "Norway",
            "Oman",
            "Pakistan",
            "Palau",
            "Palestine, State of",
            "Panama",
            "Papua New Guinea",
            "Paraguay",
            "Peru",
            "Philippines (the)",
            "Pitcairn",
            "Poland",
            "Portugal",
            "Puerto Rico",
            "Qatar",
            "Republic of North Macedonia",
            "Romania",
            "Russian Federation (the)",
            "Rwanda",
            "Réunion",
            "Saint Barthélemy",
            "Saint Helena, Ascension and Tristan da Cunha",
            "Saint Kitts and Nevis",
            "Saint Lucia",
            "Saint Martin (French part)",
            "Saint Pierre and Miquelon",
            "Saint Vincent and the Grenadines",
            "Samoa",
            "San Marino",
            "Sao Tome and Principe",
            "Saudi Arabia",
            "Senegal",
            "Serbia",
            "Seychelles",
            "Sierra Leone",
            "Singapore",
            "Sint Maarten (Dutch part)",
            "Slovakia",
            "Slovenia",
            "Solomon Islands",
            "Somalia",
            "South Africa",
            "South Georgia and the South Sandwich Islands",
            "South Sudan",
            "Spain",
            "Sri Lanka",
            "Sudan (the)",
            "Suriname",
            "Svalbard and Jan Mayen",
            "Sweden",
            "Switzerland",
            "Syrian Arab Republic",
            "Taiwan",
            "Tajikistan",
            "Tanzania, United Republic of",
            "Thailand",
            "Timor-Leste",
            "Togo",
            "Tokelau",
            "Tonga",
            "Trinidad and Tobago",
            "Tunisia",
            "Turkey",
            "Turkmenistan",
            "Turks and Caicos Islands (the)",
            "Tuvalu",
            "Uganda",
            "Ukraine",
            "United Arab Emirates (the)",
            "United Kingdom of Great Britain and Northern Ireland (the)",
            "United States Minor Outlying Islands (the)",
            "United States of America (the)",
            "Uruguay",
            "Uzbekistan",
            "Vanuatu",
            "Venezuela (Bolivarian Republic of)",
            "Viet Nam",
            "Virgin Islands (British)",
            "Virgin Islands (U.S.)",
            "Wallis and Futuna",
            "Western Sahara",
            "Yemen",
            "Zambia",
            "Zimbabwe",
            "Åland Islands"
        ];
        this.initializeForm();
        this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
    }
    initializeForm() {
        this.registerForm.controls.password.valueChanges.subscribe((value) => {
            this.registerForm.controls.confirmPassword.updateValueAndValidity();
        });
    }
    // custom validator of type ValidatorFn
    passwordConfirming(control) {
        var _a, _b;
        if (control) {
            if (control.value != ((_b = (_a = control.parent) === null || _a === void 0 ? void 0 : _a.get('password')) === null || _b === void 0 ? void 0 : _b.value)) {
                return { passwordMismatch: true };
            }
        }
        return null;
    }
    ngOnInit() {
    }
    register() {
        this.accountService.register(this.registerForm.value).subscribe(response => {
            if (response) {
                this.router.navigateByUrl('users');
                this.userService.deleteCachedValues();
            }
        }, (error) => {
        });
    }
}
RegisterComponent.ɵfac = function RegisterComponent_Factory(t) { return new (t || RegisterComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_account_service__WEBPACK_IMPORTED_MODULE_0__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_4__.ToastrService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_forms__WEBPACK_IMPORTED_MODULE_3__.FormBuilder), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_5__.Router), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_1__.UserService)); };
RegisterComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({ type: RegisterComponent, selectors: [["app-register"]], decls: 4, vars: 1, consts: [[1, "container"], [1, "row", "justify-content-center"], [1, "col-5"], ["class", "mt-4", "autocomplete", "off", 3, "formGroup", "ngSubmit", 4, "ngIf"], ["autocomplete", "off", 1, "mt-4", 3, "formGroup", "ngSubmit"], [1, "text-center", "text-secondary"], [1, "row"], ["appearance", "fill", 1, "col"], ["matInput", "", "formControlName", "firstName"], ["matInput", "", "formControlName", "lastName"], ["appearance", "fill"], ["matInput", "", "formControlName", "username"], ["matInput", "", "formControlName", "dateOfBirth", 3, "max", "matDatepicker"], ["matSuffix", "", 3, "for"], ["picker", ""], ["formControlName", "sex"], ["value", "m"], ["value", "f"], ["formControlName", "interest"], ["value", "b"], ["formControlName", "country"], [3, "value", 4, "ngFor", "ngForOf"], ["formControlName", "city", "matInput", "", "placeholder", "Ex. Cairo"], ["matInput", "", "formControlName", "email"], ["matInput", "", "formControlName", "password"], ["matInput", "", "formControlName", "confirmPassword"], [1, "mt-3"], ["href", "#"], [1, "form-group", "text-center", "my-3"], ["mat-raised-button", "", "color", "primary", "type", "submit", 3, "disabled"], [3, "value"]], template: function RegisterComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 0)(1, "div", 1)(2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](3, RegisterComponent_form_3_Template, 104, 6, "form", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](3);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.registerForm);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_6__.NgIf, _angular_forms__WEBPACK_IMPORTED_MODULE_3__["ɵNgNoValidate"], _angular_forms__WEBPACK_IMPORTED_MODULE_3__.NgControlStatusGroup, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.FormGroupDirective, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_7__.MatFormField, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_7__.MatLabel, _angular_material_input__WEBPACK_IMPORTED_MODULE_8__.MatInput, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.DefaultValueAccessor, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.NgControlStatus, _angular_forms__WEBPACK_IMPORTED_MODULE_3__.FormControlName, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_7__.MatError, _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_9__.MatDatepickerInput, _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_9__.MatDatepickerToggle, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_7__.MatSuffix, _angular_material_datepicker__WEBPACK_IMPORTED_MODULE_9__.MatDatepicker, _angular_material_select__WEBPACK_IMPORTED_MODULE_10__.MatSelect, _angular_material_core__WEBPACK_IMPORTED_MODULE_11__.MatOption, _angular_common__WEBPACK_IMPORTED_MODULE_6__.NgForOf, _angular_material_button__WEBPACK_IMPORTED_MODULE_12__.MatButton], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJyZWdpc3Rlci5jb21wb25lbnQuY3NzIn0= */"] });


/***/ }),

/***/ 6883:
/*!**************************************************************!*\
  !*** ./src/app/users/photo-editor/photo-editor.component.ts ***!
  \**************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "PhotoEditorComponent": () => (/* binding */ PhotoEditorComponent)
/* harmony export */ });
/* harmony import */ var ng2_file_upload__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ng2-file-upload */ 8356);
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var _angular_cdk_drag_drop__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/cdk/drag-drop */ 4941);
/* harmony import */ var src_environments_environment__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/environments/environment */ 2340);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var src_app_services_account_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/_services/account.service */ 7110);
/* harmony import */ var src_app_services_user_service__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/_services/user.service */ 5089);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_material_icon__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/material/icon */ 5590);
/* harmony import */ var _angular_material_table__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/table */ 7217);
/* harmony import */ var _angular_material_progress_bar__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/material/progress-bar */ 9782);














function PhotoEditorComponent_div_0_div_1_button_3_Template(rf, ctx) { if (rf & 1) {
    const _r8 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "button", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function PhotoEditorComponent_div_0_div_1_button_3_Template_button_click_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r8); const photo_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"]().$implicit; const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](2); return ctx_r6.deletePhoto(photo_r4.id); });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](1, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](2, "delete");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
} }
function PhotoEditorComponent_div_0_div_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "div", 9)(1, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](2, "img", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](3, PhotoEditorComponent_div_0_div_1_button_3_Template, 3, 0, "button", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
} if (rf & 2) {
    const photo_r4 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("cdkDragData", photo_r4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpropertyInterpolate"]("src", photo_r4.url, _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵsanitizeUrl"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpropertyInterpolate"]("alt", photo_r4.url);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", photo_r4.order != 0);
} }
function PhotoEditorComponent_div_0_Template(rf, ctx) { if (rf & 1) {
    const _r10 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "div", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("cdkDropListDropped", function PhotoEditorComponent_div_0_Template_div_cdkDropListDropped_0_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r10); const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r9.drop($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](1, PhotoEditorComponent_div_0_div_1_Template, 4, 4, "div", 8);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("cdkDropListData", ctx_r0.user.photos);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngForOf", ctx_r0.user.photos);
} }
function PhotoEditorComponent_div_13_table_1_th_2_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "th", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](1, " Name ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} }
function PhotoEditorComponent_div_13_table_1_td_3_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "td", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r19 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate1"](" ", item_r19 == null ? null : item_r19.file == null ? null : item_r19.file.name, " ");
} }
function PhotoEditorComponent_div_13_table_1_th_5_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "th", 29);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](1, " Size ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} }
function PhotoEditorComponent_div_13_table_1_td_6_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "td", 30);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipe"](2, "number");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} if (rf & 2) {
    const item_r20 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate1"]("", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipeBind2"](2, 1, (item_r20 == null ? null : item_r20.file == null ? null : item_r20.file.size) / 1024 / 1024, ".2"), " MB");
} }
function PhotoEditorComponent_div_13_table_1_tr_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](0, "tr", 31);
} }
function PhotoEditorComponent_div_13_table_1_tr_8_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](0, "tr", 32);
} }
const _c0 = function () { return ["name", "size"]; };
function PhotoEditorComponent_div_13_table_1_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "table", 22);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementContainerStart"](1, 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](2, PhotoEditorComponent_div_13_table_1_th_2_Template, 2, 0, "th", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](3, PhotoEditorComponent_div_13_table_1_td_3_Template, 2, 1, "td", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementContainerStart"](4, 26);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](5, PhotoEditorComponent_div_13_table_1_th_5_Template, 2, 0, "th", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](6, PhotoEditorComponent_div_13_table_1_td_6_Template, 3, 4, "td", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementContainerEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](7, PhotoEditorComponent_div_13_table_1_tr_7_Template, 1, 0, "tr", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](8, PhotoEditorComponent_div_13_table_1_tr_8_Template, 1, 0, "tr", 28);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} if (rf & 2) {
    const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("dataSource", ctx_r11.uploader.queue);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("matHeaderRowDef", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpureFunction0"](3, _c0));
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("matRowDefColumns", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpureFunction0"](4, _c0));
} }
function PhotoEditorComponent_div_13_mat_progress_bar_4_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](0, "mat-progress-bar", 33);
} if (rf & 2) {
    const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("value", ctx_r12.uploader.progress);
} }
function PhotoEditorComponent_div_13_Template(rf, ctx) { if (rf & 1) {
    const _r23 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "div", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](1, PhotoEditorComponent_div_13_table_1_Template, 9, 5, "table", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](2, "div", 16)(3, "div");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](4, PhotoEditorComponent_div_13_mat_progress_bar_4_Template, 1, 1, "mat-progress-bar", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](5, "div", 18)(6, "div")(7, "button", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function PhotoEditorComponent_div_13_Template_button_click_7_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r23); const ctx_r22 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r22.uploader.uploadAll(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](8, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](9, "upload");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](10, " Upload all ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](11, "button", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function PhotoEditorComponent_div_13_Template_button_click_11_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r23); const ctx_r24 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r24.uploader.cancelAll(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](12, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](13, "block");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](14, " Cancel all ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](15, "button", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function PhotoEditorComponent_div_13_Template_button_click_15_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r23); const ctx_r25 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r25.uploader.clearQueue(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](16, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](17, "delete");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](18, " Remove all ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()()()();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", ctx_r2.uploader.options.isHTML5);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", ctx_r2.uploader.isUploading);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("disabled", !ctx_r2.uploader.getNotUploadedItems().length);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("disabled", !ctx_r2.uploader.isUploading);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("disabled", !ctx_r2.uploader.queue.length);
} }
class PhotoEditorComponent {
    constructor(accountService, userService) {
        this.accountService = accountService;
        this.userService = userService;
        this.user = undefined;
        this.baseUrl = src_environments_environment__WEBPACK_IMPORTED_MODULE_0__.environment.apiUrl;
        this.account = null;
        this.isOrderChanged = false;
        this.uploader = new ng2_file_upload__WEBPACK_IMPORTED_MODULE_4__.FileUploader({});
        this.hasBaseDropzoneOver = false;
        accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_5__.take)(1)).subscribe(acc => {
            if (acc) {
                this.account = acc;
            }
        });
    }
    isOrderChangedFunc() {
        var _a, _b;
        let initialPhotos = (_a = this.account) === null || _a === void 0 ? void 0 : _a.userData.photos;
        for (let index = 0; index < initialPhotos.length; index++) {
            if (((_b = this.user) === null || _b === void 0 ? void 0 : _b.photos[index].id) != initialPhotos[index].id) {
                return true;
            }
        }
        return false;
    }
    initializeUploader() {
        var _a;
        this.uploader = new ng2_file_upload__WEBPACK_IMPORTED_MODULE_4__.FileUploader({
            url: this.baseUrl + 'users/photo/upload',
            authToken: 'Bearer ' + ((_a = this.account) === null || _a === void 0 ? void 0 : _a.token),
            isHTML5: true,
            allowedFileType: ['image'],
            removeAfterUpload: true,
            autoUpload: false,
            maxFileSize: 10 * 1024 * 1024
        });
        this.uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };
        this.uploader.onSuccessItem = (item, response, status, headers) => {
            var _a, _b, _c;
            if (response) {
                const photo = JSON.parse(response);
                (_b = (_a = this.user) === null || _a === void 0 ? void 0 : _a.photos) === null || _b === void 0 ? void 0 : _b.push(photo);
                if (this.account) {
                    (_c = this.user) === null || _c === void 0 ? void 0 : _c.photos.forEach(p => { var _a; return (_a = this.account) === null || _a === void 0 ? void 0 : _a.userData.photos.push(Object.assign({}, p)); });
                    this.accountService.setCurrentUser(this.account);
                }
            }
        };
    }
    reorderPhotos() {
        var _a;
        let counter = 0;
        (_a = this.user) === null || _a === void 0 ? void 0 : _a.photos.forEach(p => {
            p.order = counter;
            counter++;
        });
        if (this.user) {
            this.userService.reorderPhotos(this.user.photos).subscribe(response => {
                if (response && this.user && this.account) {
                    this.account.userData.photos = [];
                    // copying to make sure that the account object 
                    // is isolated from any other incoming unsaved changes
                    this.user.photos.forEach(p => { var _a; return (_a = this.account) === null || _a === void 0 ? void 0 : _a.userData.photos.push(Object.assign({}, p)); });
                    this.accountService.setCurrentUser(this.account);
                    this.isOrderChanged = false;
                }
            });
        }
    }
    deletePhoto(photoId) {
        this.userService.deletePhoto(photoId).subscribe(() => {
            if (this.account && this.user) {
                const index = this.user.photos.findIndex(p => p.id === photoId);
                this.user.photos = this.user.photos.filter(p => p.id !== photoId);
                // reassign the order values of the pictures after the deleted picture.
                for (let i = index + 1; i < this.user.photos.length; i++) {
                    this.user.photos[i].order--;
                }
                this.account.userData.photos = [];
                // copying to make sure that the account object 
                // is isolated from any other incoming unsaved changes
                this.user.photos.forEach(p => { var _a; return (_a = this.account) === null || _a === void 0 ? void 0 : _a.userData.photos.push(Object.assign({}, p)); });
                this.accountService.setCurrentUser(this.account);
            }
        });
    }
    drop(event) {
        var _a;
        (0,_angular_cdk_drag_drop__WEBPACK_IMPORTED_MODULE_6__.moveItemInArray)((_a = this.user) === null || _a === void 0 ? void 0 : _a.photos, event.previousIndex, event.currentIndex);
        this.isOrderChanged = this.isOrderChangedFunc();
    }
    fileOverBase(event) {
        this.hasBaseDropzoneOver = event;
    }
    ngOnInit() {
        this.initializeUploader();
    }
}
PhotoEditorComponent.ɵfac = function PhotoEditorComponent_Factory(t) { return new (t || PhotoEditorComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](src_app_services_account_service__WEBPACK_IMPORTED_MODULE_1__.AccountService), _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](src_app_services_user_service__WEBPACK_IMPORTED_MODULE_2__.UserService)); };
PhotoEditorComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdefineComponent"]({ type: PhotoEditorComponent, selectors: [["app-photo-editor"]], inputs: { user: "user" }, decls: 14, vars: 4, consts: [["cdkDropList", "", "cdkDropListOrientation", "horizontal", "class", "mt-4 row drag-list justify-content-center mx-0", 3, "cdkDropListData", "cdkDropListDropped", 4, "ngIf"], ["id", "inputFile", "type", "file", "ng2FileSelect", "", "multiple", "", 2, "display", "none", 3, "uploader"], ["fileUpload", ""], [1, "mt-2", "d-grid", "justify-content-center"], ["mat-raised-button", "", "color", "primary", 1, "me-2", 3, "disabled", "click"], ["mat-raised-button", "", "color", "accent", 3, "click"], ["class", "row mt-4 mx-0", "style", "margin-bottom: 40px", 4, "ngIf"], ["cdkDropList", "", "cdkDropListOrientation", "horizontal", 1, "mt-4", "row", "drag-list", "justify-content-center", "mx-0", 3, "cdkDropListData", "cdkDropListDropped"], ["class", "col-2 drag-box mt-2", "cdkDrag", "", 3, "cdkDragData", 4, "ngFor", "ngForOf"], ["cdkDrag", "", 1, "col-2", "drag-box", "mt-2", 3, "cdkDragData"], [1, "text-center"], [1, "img-thumbnail", "p-1", 3, "src", "alt"], ["mat-icon-button", "", "color", "warn", 3, "click", 4, "ngIf"], ["mat-icon-button", "", "color", "warn", 3, "click"], [1, "row", "mt-4", "mx-0", 2, "margin-bottom", "40px"], ["mat-table", "", "class", "mat-elevation-z8", 3, "dataSource", 4, "ngIf"], [1, "mt-4"], ["mode", "determinate", 3, "value", 4, "ngIf"], [1, "mt-4", "d-grid", "justify-content-center"], ["mat-raised-button", "", "color", "primary", "type", "button", 1, "me-2", 3, "disabled", "click"], ["type", "button", "mat-raised-button", "", "color", "accent", 1, "me-2", 3, "disabled", "click"], ["type", "button", "mat-raised-button", "", "color", "warn", 3, "disabled", "click"], ["mat-table", "", 1, "mat-elevation-z8", 3, "dataSource"], ["matColumnDef", "name"], ["mat-header-cell", "", 4, "matHeaderCellDef"], ["mat-cell", "", 4, "matCellDef"], ["matColumnDef", "size"], ["mat-header-row", "", 4, "matHeaderRowDef"], ["mat-row", "", 4, "matRowDef", "matRowDefColumns"], ["mat-header-cell", ""], ["mat-cell", ""], ["mat-header-row", ""], ["mat-row", ""], ["mode", "determinate", 3, "value"]], template: function PhotoEditorComponent_Template(rf, ctx) { if (rf & 1) {
        const _r26 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵgetCurrentView"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](0, PhotoEditorComponent_div_0_Template, 2, 2, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](1, "input", 1, 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](3, "div", 3)(4, "div")(5, "button", 4);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function PhotoEditorComponent_Template_button_click_5_listener() { return ctx.reorderPhotos(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](6, "mat-icon");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](7, "save");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](8, " Save ");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](9, "button", 5);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("click", function PhotoEditorComponent_Template_button_click_9_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r26); const _r1 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵreference"](2); return _r1.click(); });
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](10, "mat-icon");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](11, "add_photo_alternate");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](12, " Add Picture ");
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()();
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](13, PhotoEditorComponent_div_13_Template, 19, 5, "div", 6);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", ctx.user);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("uploader", ctx.uploader);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](4);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("disabled", !ctx.isOrderChanged);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](8);
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", ctx.uploader == null ? null : ctx.uploader.queue == null ? null : ctx.uploader.queue.length);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_7__.NgIf, _angular_cdk_drag_drop__WEBPACK_IMPORTED_MODULE_6__.CdkDropList, _angular_common__WEBPACK_IMPORTED_MODULE_7__.NgForOf, _angular_cdk_drag_drop__WEBPACK_IMPORTED_MODULE_6__.CdkDrag, _angular_material_button__WEBPACK_IMPORTED_MODULE_8__.MatButton, _angular_material_icon__WEBPACK_IMPORTED_MODULE_9__.MatIcon, ng2_file_upload__WEBPACK_IMPORTED_MODULE_4__.FileSelectDirective, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatTable, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatColumnDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatHeaderCellDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatHeaderCell, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatCellDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatCell, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatHeaderRowDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatHeaderRow, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatRowDef, _angular_material_table__WEBPACK_IMPORTED_MODULE_10__.MatRow, _angular_material_progress_bar__WEBPACK_IMPORTED_MODULE_11__.MatProgressBar], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_7__.DecimalPipe], styles: [".cdk-drag-preview[_ngcontent-%COMP%] {\r\n    box-sizing: border-box;\r\n    border-radius: 4px;\r\n    box-shadow: 0 5px 5px -3px rgba(0, 0, 0, 0.2),\r\n                0 8px 10px 1px rgba(0, 0, 0, 0.14),\r\n                0 3px 14px 2px rgba(0, 0, 0, 0.12);\r\n  }\r\n  \r\n  .cdk-drag-placeholder[_ngcontent-%COMP%] {\r\n    opacity: 0;\r\n  }\r\n  \r\n  .cdk-drag-animating[_ngcontent-%COMP%] {\r\n    transition: transform 250ms cubic-bezier(0, 0, 0.2, 1);\r\n  }\r\n  \r\n  .example-box[_ngcontent-%COMP%]:last-child {\r\n    border: none;\r\n  }\r\n  \r\n  .drag-list.cdk-drop-list-dragging[_ngcontent-%COMP%]   .drag-box[_ngcontent-%COMP%]:not(.cdk-drag-placeholder) {\r\n    transition: transform 250ms cubic-bezier(0, 0, 0.2, 1);\r\n  }\r\n  \r\n  img.img-thumbnail[_ngcontent-%COMP%]{\r\n  height: 100px;\r\n  min-width: 100px !important;\r\n  margin-bottom: 2px;\r\n}\r\n  \r\n  .nv-file-over[_ngcontent-%COMP%]{\r\n  border: dotted 3px red;\r\n}\r\n  \r\n  input[type=file][_ngcontent-%COMP%]{\r\n  color: transparent;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInBob3RvLWVkaXRvci5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJFQUFFO0lBQ0Usc0JBQXNCO0lBQ3RCLGtCQUFrQjtJQUNsQjs7a0RBRThDO0VBQ2hEOztFQUVBO0lBQ0UsVUFBVTtFQUNaOztFQUVBO0lBQ0Usc0RBQXNEO0VBQ3hEOztFQUVBO0lBQ0UsWUFBWTtFQUNkOztFQUVBO0lBQ0Usc0RBQXNEO0VBQ3hEOztFQUNGO0VBQ0UsYUFBYTtFQUNiLDJCQUEyQjtFQUMzQixrQkFBa0I7QUFDcEI7O0VBQ0E7RUFDRSxzQkFBc0I7QUFDeEI7O0VBQ0E7RUFDRSxrQkFBa0I7QUFDcEIiLCJmaWxlIjoicGhvdG8tZWRpdG9yLmNvbXBvbmVudC5jc3MiLCJzb3VyY2VzQ29udGVudCI6WyIgIC5jZGstZHJhZy1wcmV2aWV3IHtcclxuICAgIGJveC1zaXppbmc6IGJvcmRlci1ib3g7XHJcbiAgICBib3JkZXItcmFkaXVzOiA0cHg7XHJcbiAgICBib3gtc2hhZG93OiAwIDVweCA1cHggLTNweCByZ2JhKDAsIDAsIDAsIDAuMiksXHJcbiAgICAgICAgICAgICAgICAwIDhweCAxMHB4IDFweCByZ2JhKDAsIDAsIDAsIDAuMTQpLFxyXG4gICAgICAgICAgICAgICAgMCAzcHggMTRweCAycHggcmdiYSgwLCAwLCAwLCAwLjEyKTtcclxuICB9XHJcbiAgXHJcbiAgLmNkay1kcmFnLXBsYWNlaG9sZGVyIHtcclxuICAgIG9wYWNpdHk6IDA7XHJcbiAgfVxyXG4gIFxyXG4gIC5jZGstZHJhZy1hbmltYXRpbmcge1xyXG4gICAgdHJhbnNpdGlvbjogdHJhbnNmb3JtIDI1MG1zIGN1YmljLWJlemllcigwLCAwLCAwLjIsIDEpO1xyXG4gIH1cclxuICBcclxuICAuZXhhbXBsZS1ib3g6bGFzdC1jaGlsZCB7XHJcbiAgICBib3JkZXI6IG5vbmU7XHJcbiAgfVxyXG4gIFxyXG4gIC5kcmFnLWxpc3QuY2RrLWRyb3AtbGlzdC1kcmFnZ2luZyAuZHJhZy1ib3g6bm90KC5jZGstZHJhZy1wbGFjZWhvbGRlcikge1xyXG4gICAgdHJhbnNpdGlvbjogdHJhbnNmb3JtIDI1MG1zIGN1YmljLWJlemllcigwLCAwLCAwLjIsIDEpO1xyXG4gIH1cclxuaW1nLmltZy10aHVtYm5haWx7XHJcbiAgaGVpZ2h0OiAxMDBweDtcclxuICBtaW4td2lkdGg6IDEwMHB4ICFpbXBvcnRhbnQ7XHJcbiAgbWFyZ2luLWJvdHRvbTogMnB4O1xyXG59XHJcbi5udi1maWxlLW92ZXJ7XHJcbiAgYm9yZGVyOiBkb3R0ZWQgM3B4IHJlZDtcclxufVxyXG5pbnB1dFt0eXBlPWZpbGVde1xyXG4gIGNvbG9yOiB0cmFuc3BhcmVudDtcclxufSJdfQ== */"] });


/***/ }),

/***/ 9412:
/*!********************************************************!*\
  !*** ./src/app/users/user-card/user-card.component.ts ***!
  \********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserCardComponent": () => (/* binding */ UserCardComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var src_app_services_user_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! src/app/_services/user.service */ 5089);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ngx-toastr */ 4101);
/* harmony import */ var src_app_services_presence_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/_services/presence.service */ 3238);
/* harmony import */ var _angular_material_card__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/material/card */ 1961);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var _angular_material_icon__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/material/icon */ 5590);
/* harmony import */ var ngx_timeago__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! ngx-timeago */ 2699);












function UserCardComponent_mat_card_subtitle_4_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-card-subtitle");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](1, "span", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, " Active now");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
  }
}

function UserCardComponent_mat_card_subtitle_6_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-card-subtitle");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](2, "timeago");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const ctx_r1 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](2, 1, ctx_r1.getLoacaleDateTime(ctx_r1.user.lastActive)));
  }
}

function UserCardComponent_button_14_Template(rf, ctx) {
  if (rf & 1) {
    const _r5 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();

    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "button", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("click", function UserCardComponent_button_14_Template_button_click_0_listener() {
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r5);
      const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
      return ctx_r4.addLike(ctx_r4.user);
    });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](1, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, "favorite");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
  }
}

function UserCardComponent_button_15_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "button", 8)(1, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, "chat");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
  }

  if (rf & 2) {
    const ctx_r3 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("state", ctx_r3.user);
  }
}

class UserCardComponent {
  constructor(userService, toastr, presenceService) {
    this.userService = userService;
    this.toastr = toastr;
    this.presenceService = presenceService;
    this.isLiked = false;
    this.liked = new _angular_core__WEBPACK_IMPORTED_MODULE_2__.EventEmitter();
    this.user = {
      id: 0,
      username: '',
      firstName: '',
      lastName: '',
      sex: '',
      interest: '',
      age: 0,
      created: new Date(),
      lastActive: new Date(),
      bio: '',
      city: '',
      country: '',
      photos: [],
      roles: []
    };
  }

  ngOnInit() {}

  getLoacaleDateTime(d) {
    var localDate = new Date(d.toString() + 'Z');
    return localDate;
  }

  addLike(user) {
    this.userService.like(user.username).subscribe(r => {
      this.toastr.success('You have liked ' + user.firstName);

      if (r == true) {
        this.toastr.success("You have a new match!");
      }

      this.liked.emit();
    });
  }

}

UserCardComponent.ɵfac = function UserCardComponent_Factory(t) {
  return new (t || UserCardComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](src_app_services_user_service__WEBPACK_IMPORTED_MODULE_0__.UserService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_3__.ToastrService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](src_app_services_presence_service__WEBPACK_IMPORTED_MODULE_1__.PresenceService));
};

UserCardComponent.ɵcmp = /*@__PURE__*/_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({
  type: UserCardComponent,
  selectors: [["app-user-card"]],
  inputs: {
    isLiked: "isLiked",
    user: "user"
  },
  outputs: {
    liked: "liked"
  },
  decls: 16,
  vars: 12,
  consts: [[4, "ngIf"], ["mat-card-image", "", "alt", "Profile picture", 3, "src"], [1, "text-center"], ["mat-button", "", "color", "primary", 1, "me-2", 3, "routerLink"], ["mat-button", "", "color", "accent", "class", "me-2", 3, "click", 4, "ngIf"], ["mat-button", "", "color", "primary", "routerLink", "/messages", 3, "state", 4, "ngIf"], [1, "online"], ["mat-button", "", "color", "accent", 1, "me-2", 3, "click"], ["mat-button", "", "color", "primary", "routerLink", "/messages", 3, "state"]],
  template: function UserCardComponent_Template(rf, ctx) {
    if (rf & 1) {
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-card")(1, "mat-card-header")(2, "mat-card-title");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](3);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](4, UserCardComponent_mat_card_subtitle_4_Template, 3, 0, "mat-card-subtitle", 0);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](5, "async");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](6, UserCardComponent_mat_card_subtitle_6_Template, 3, 3, "mat-card-subtitle", 0);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](7, "async");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](8, "img", 1)(9, "mat-card-content");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](10, "mat-card-actions", 2)(11, "button", 3)(12, "mat-icon");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](13, "person");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](14, UserCardComponent_button_14_Template, 3, 0, "button", 4);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](15, UserCardComponent_button_15_Template, 3, 1, "button", 5);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    }

    if (rf & 2) {
      let tmp_1_0;
      let tmp_2_0;
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](3);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate2"]("", ctx.user.firstName, " ", ctx.user.lastName, "");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", (tmp_1_0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](5, 8, ctx.presenceService.onlineUsers$)) == null ? null : tmp_1_0.includes(ctx.user.username));
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", !((tmp_2_0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](7, 10, ctx.presenceService.onlineUsers$)) == null ? null : tmp_2_0.includes(ctx.user.username)));
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpropertyInterpolate"]("src", (ctx.user.photos[0] == null ? null : ctx.user.photos[0].url) || "./assets/Pictures/profile-placeholder.png", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵsanitizeUrl"]);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](3);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpropertyInterpolate1"]("routerLink", "/users/username/", ctx.user.username, "");
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](3);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", !ctx.isLiked);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.isLiked);
    }
  },
  directives: [_angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCard, _angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCardHeader, _angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCardTitle, _angular_common__WEBPACK_IMPORTED_MODULE_5__.NgIf, _angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCardSubtitle, _angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCardImage, _angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCardContent, _angular_material_card__WEBPACK_IMPORTED_MODULE_4__.MatCardActions, _angular_material_button__WEBPACK_IMPORTED_MODULE_6__.MatButton, _angular_router__WEBPACK_IMPORTED_MODULE_7__.RouterLink, _angular_material_icon__WEBPACK_IMPORTED_MODULE_8__.MatIcon],
  pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_5__.AsyncPipe, ngx_timeago__WEBPACK_IMPORTED_MODULE_9__.TimeagoPipe],
  styles: ["@keyframes fa-blink {\r\n    0% {opacity: 1;}\r\n    100% {opacity: 0.4;}\r\n}\r\n.online[_ngcontent-%COMP%]{\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: rgb(1, 189, 42);\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInVzZXItY2FyZC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0lBQ0ksSUFBSSxVQUFVLENBQUM7SUFDZixNQUFNLFlBQVksQ0FBQztBQUN2QjtBQUNBO0lBQ0ksWUFBWTtJQUNaLFdBQVc7SUFDWCxpQ0FBaUM7SUFDakMsa0JBQWtCO0lBQ2xCLHFCQUFxQjtBQUN6QiIsImZpbGUiOiJ1c2VyLWNhcmQuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIkBrZXlmcmFtZXMgZmEtYmxpbmsge1xyXG4gICAgMCUge29wYWNpdHk6IDE7fVxyXG4gICAgMTAwJSB7b3BhY2l0eTogMC40O31cclxufVxyXG4ub25saW5le1xyXG4gICAgaGVpZ2h0OiAxMHB4O1xyXG4gICAgd2lkdGg6IDEwcHg7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiByZ2IoMSwgMTg5LCA0Mik7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbn0iXX0= */"]
});

/***/ }),

/***/ 2545:
/*!************************************************************!*\
  !*** ./src/app/users/user-detail/user-detail.component.ts ***!
  \************************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserDetailComponent": () => (/* binding */ UserDetailComponent)
/* harmony export */ });
/* harmony import */ var _kolkov_ngx_gallery__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @kolkov/ngx-gallery */ 9222);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../_services/user.service */ 5089);
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ 2816);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ngx-toastr */ 4101);
/* harmony import */ var src_app_services_presence_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! src/app/_services/presence.service */ 3238);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_card__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/card */ 1961);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _angular_material_icon__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/material/icon */ 5590);
/* harmony import */ var _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/tabs */ 2379);
/* harmony import */ var ngx_timeago__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! ngx-timeago */ 2699);














function UserDetailComponent_div_0_mat_card_subtitle_5_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-card-subtitle");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](1, "span", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, " Active now");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
  }
}

function UserDetailComponent_div_0_mat_card_subtitle_7_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-card-subtitle");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](2, "timeago");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](2, 1, ctx_r2.getLoacaleDateTime(ctx_r2.user.lastActive)));
  }
}

function UserDetailComponent_div_0_button_28_Template(rf, ctx) {
  if (rf & 1) {
    const _r7 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();

    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "button", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("click", function UserDetailComponent_div_0_button_28_Template_button_click_0_listener() {
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r7);
      const ctx_r6 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2);
      return ctx_r6.addLike();
    });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](1, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, "favorite");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](3, " LIKE ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
  }
}

function UserDetailComponent_div_0_button_29_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "button", 16)(1, "mat-icon");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, "chat");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](3, " Chat ");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
  }

  if (rf & 2) {
    const ctx_r4 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("state", ctx_r4.user);
  }
}

function UserDetailComponent_div_0_div_32_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 11)(1, "h2");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](2, "Bio");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](3, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
  }

  if (rf & 2) {
    const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](ctx_r5.user.bio);
  }
}

function UserDetailComponent_div_0_Template(rf, ctx) {
  if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 1)(1, "mat-card", 2)(2, "mat-card-header")(3, "mat-card-title");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](5, UserDetailComponent_div_0_mat_card_subtitle_5_Template, 3, 0, "mat-card-subtitle", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](6, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](7, UserDetailComponent_div_0_mat_card_subtitle_7_Template, 3, 3, "mat-card-subtitle", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](8, "async");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](9, "img", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](10, "mat-card-content")(11, "div")(12, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](13, "Location:");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](14, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](15);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](16, "div")(17, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](18, "Age:");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](19, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](20);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](21, "div")(22, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](23, "Member since:");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](24, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](25);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipe"](26, "date");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](27, "mat-card-actions", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](28, UserDetailComponent_div_0_button_28_Template, 4, 0, "button", 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](29, UserDetailComponent_div_0_button_29_Template, 4, 1, "button", 7);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](30, "mat-tab-group", 8)(31, "mat-tab", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](32, UserDetailComponent_div_0_div_32_Template, 5, 1, "div", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](33, "div", 11)(34, "h2");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](35, "Interested in");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](36, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](37);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](38, "mat-tab", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelement"](39, "ngx-gallery", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
  }

  if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    let tmp_1_0;
    let tmp_2_0;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate2"]("", ctx_r0.user.firstName, " ", ctx_r0.user.lastName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", (tmp_1_0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](6, 15, ctx_r0.presenceService.onlineUsers$)) == null ? null : tmp_1_0.includes(ctx_r0.user.username));
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", !((tmp_2_0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](8, 17, ctx_r0.presenceService.onlineUsers$)) == null ? null : tmp_2_0.includes(ctx_r0.user.username)));
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpropertyInterpolate"]("src", (ctx_r0.user.photos[0] == null ? null : ctx_r0.user.photos[0].url) || "./assets/Pictures/profile-placeholder.png", _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵsanitizeUrl"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate2"]("", ctx_r0.user.city, ", ", ctx_r0.user.country, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](ctx_r0.user.age);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵpipeBind1"](26, 19, ctx_r0.getLoacaleDateTime(ctx_r0.user.created)));
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", !ctx_r0.isLiked);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r0.isMatch);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r0.user.bio);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](ctx_r0.formatInterest(ctx_r0.user.interest));
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("options", ctx_r0.galleryOptions)("images", ctx_r0.galleryImages);
  }
}

class UserDetailComponent {
  constructor(userService, route, toastr, presenceService) {
    this.userService = userService;
    this.route = route;
    this.toastr = toastr;
    this.presenceService = presenceService;
    this.user = {
      id: 0,
      username: '',
      firstName: '',
      lastName: '',
      sex: '',
      interest: '',
      age: 0,
      created: new Date(),
      lastActive: new Date(),
      bio: '',
      city: '',
      country: '',
      photos: [],
      roles: []
    };
    this.isLiked = true;
    this.isMatch = false;
    this.galleryOptions = [];
    this.galleryImages = [];
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: _kolkov_ngx_gallery__WEBPACK_IMPORTED_MODULE_3__.NgxGalleryAnimation.Slide,
      preview: false
    }];
  }

  addLike() {
    this.userService.like(this.user.username).subscribe(r => {
      this.isLiked = true;
      this.toastr.success('You have liked ' + this.user.firstName);

      if (r == true) {
        this.toastr.success("You have a new match!");
      }
    });
  }

  getLoacaleDateTime(d) {
    var localDate = new Date(d.toString() + 'Z');
    return localDate;
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data.user;
      this.setImages();
      this.userService.getIsLiked(this.user.username).subscribe(r => {
        this.isLiked = r;
      });
      this.userService.getIsMatch(this.user.username).subscribe(r => this.isMatch = r);
    });
  }

  formatInterest(interest) {
    if (interest === 'f') {
      return 'Females';
    } else if (interest === 'm') {
      return 'Males';
    } else {
      return 'Both females and males';
    }
  }

  setImages() {
    for (const photo of this.user.photos) {
      this.galleryImages.push({
        small: photo === null || photo === void 0 ? void 0 : photo.url,
        medium: photo === null || photo === void 0 ? void 0 : photo.url,
        big: photo === null || photo === void 0 ? void 0 : photo.url
      });
    }
  }

}

UserDetailComponent.ɵfac = function UserDetailComponent_Factory(t) {
  return new (t || UserDetailComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_0__.UserService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_angular_router__WEBPACK_IMPORTED_MODULE_4__.ActivatedRoute), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_5__.ToastrService), _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](src_app_services_presence_service__WEBPACK_IMPORTED_MODULE_1__.PresenceService));
};

UserDetailComponent.ɵcmp = /*@__PURE__*/_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({
  type: UserDetailComponent,
  selectors: [["app-user-detail"]],
  decls: 1,
  vars: 1,
  consts: [["class", "row", 4, "ngIf"], [1, "row"], [1, "col-4"], [4, "ngIf"], ["mat-card-image", "", "alt", "profile picture", 3, "src"], [1, "text-center"], ["mat-button", "", "color", "accent", 3, "click", 4, "ngIf"], ["mat-button", "", "color", "primary", "routerLink", "/messages", 3, "state", 4, "ngIf"], ["mat-align-tabs", "center", 1, "ps-4", "col-8"], ["label", "About"], ["class", "mt-4", 4, "ngIf"], [1, "mt-4"], ["label", "Gallery"], [1, "ngx-gallery", 3, "options", "images"], [1, "online"], ["mat-button", "", "color", "accent", 3, "click"], ["mat-button", "", "color", "primary", "routerLink", "/messages", 3, "state"]],
  template: function UserDetailComponent_Template(rf, ctx) {
    if (rf & 1) {
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](0, UserDetailComponent_div_0_Template, 40, 21, "div", 0);
    }

    if (rf & 2) {
      _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.user.username);
    }
  },
  directives: [_angular_common__WEBPACK_IMPORTED_MODULE_6__.NgIf, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCard, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardHeader, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardTitle, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardSubtitle, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardImage, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardContent, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardActions, _angular_material_button__WEBPACK_IMPORTED_MODULE_8__.MatButton, _angular_material_icon__WEBPACK_IMPORTED_MODULE_9__.MatIcon, _angular_router__WEBPACK_IMPORTED_MODULE_4__.RouterLink, _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__.MatTabGroup, _angular_material_tabs__WEBPACK_IMPORTED_MODULE_10__.MatTab, _kolkov_ngx_gallery__WEBPACK_IMPORTED_MODULE_3__.NgxGalleryComponent],
  pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_6__.AsyncPipe, ngx_timeago__WEBPACK_IMPORTED_MODULE_11__.TimeagoPipe, _angular_common__WEBPACK_IMPORTED_MODULE_6__.DatePipe],
  styles: [".img-thumbnail[_ngcontent-%COMP%]{\r\n    margin: 25px;\r\n    width: 85%;\r\n    height: 85%;\r\n}\r\n.card-body[_ngcontent-%COMP%] {\r\n    padding: 0 25px;\r\n}\r\n.card-footer[_ngcontent-%COMP%] {\r\n    padding: 10px 15px;\r\n    background-color: #fff;\r\n    border-top: none;\r\n}\r\n.ngx-gallery[_ngcontent-%COMP%] {\r\n  display: inline-block;\r\n  margin-bottom: 20px;\r\n}\r\n.online[_ngcontent-%COMP%]{\r\n    height: 10px;\r\n    width: 10px;\r\n    background-color: rgb(1, 189, 42);\r\n    border-radius: 50%;\r\n    display: inline-block;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInVzZXItZGV0YWlsLmNvbXBvbmVudC5jc3MiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7SUFDSSxZQUFZO0lBQ1osVUFBVTtJQUNWLFdBQVc7QUFDZjtBQUNBO0lBQ0ksZUFBZTtBQUNuQjtBQUVBO0lBQ0ksa0JBQWtCO0lBQ2xCLHNCQUFzQjtJQUN0QixnQkFBZ0I7QUFDcEI7QUFDQTtFQUNFLHFCQUFxQjtFQUNyQixtQkFBbUI7QUFDckI7QUFDQTtJQUNJLFlBQVk7SUFDWixXQUFXO0lBQ1gsaUNBQWlDO0lBQ2pDLGtCQUFrQjtJQUNsQixxQkFBcUI7QUFDekIiLCJmaWxlIjoidXNlci1kZXRhaWwuY29tcG9uZW50LmNzcyIsInNvdXJjZXNDb250ZW50IjpbIi5pbWctdGh1bWJuYWlse1xyXG4gICAgbWFyZ2luOiAyNXB4O1xyXG4gICAgd2lkdGg6IDg1JTtcclxuICAgIGhlaWdodDogODUlO1xyXG59XHJcbi5jYXJkLWJvZHkge1xyXG4gICAgcGFkZGluZzogMCAyNXB4O1xyXG59XHJcblxyXG4uY2FyZC1mb290ZXIge1xyXG4gICAgcGFkZGluZzogMTBweCAxNXB4O1xyXG4gICAgYmFja2dyb3VuZC1jb2xvcjogI2ZmZjtcclxuICAgIGJvcmRlci10b3A6IG5vbmU7XHJcbn1cclxuLm5neC1nYWxsZXJ5IHtcclxuICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgbWFyZ2luLWJvdHRvbTogMjBweDtcclxufVxyXG4ub25saW5le1xyXG4gICAgaGVpZ2h0OiAxMHB4O1xyXG4gICAgd2lkdGg6IDEwcHg7XHJcbiAgICBiYWNrZ3JvdW5kLWNvbG9yOiByZ2IoMSwgMTg5LCA0Mik7XHJcbiAgICBib3JkZXItcmFkaXVzOiA1MCU7XHJcbiAgICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbn0iXX0= */"]
});

/***/ }),

/***/ 1923:
/*!********************************************************!*\
  !*** ./src/app/users/user-edit/user-edit.component.ts ***!
  \********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserEditComponent": () => (/* binding */ UserEditComponent)
/* harmony export */ });
/* harmony import */ var rxjs__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! rxjs */ 9295);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var ngx_toastr__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ngx-toastr */ 4101);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../_services/user.service */ 5089);
/* harmony import */ var _services_account_service__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../_services/account.service */ 7110);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_material_card__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/card */ 1961);
/* harmony import */ var _angular_material_tabs__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/material/tabs */ 2379);
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/forms */ 587);
/* harmony import */ var _angular_material_form_field__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/form-field */ 9076);
/* harmony import */ var _angular_material_input__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(/*! @angular/material/input */ 3365);
/* harmony import */ var _angular_material_select__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(/*! @angular/material/select */ 1434);
/* harmony import */ var _angular_material_core__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(/*! @angular/material/core */ 8133);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _photo_editor_photo_editor_component__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../photo-editor/photo-editor.component */ 6883);















const _c0 = ["editForm"];
function UserEditComponent_div_0_div_24_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "div", 26)(1, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](2, "Information: ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](3, " You have made changes. Any unsaved changes will be lost. ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} }
function UserEditComponent_div_0_mat_option_59_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "mat-option", 27);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
} if (rf & 2) {
    const country_r4 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("value", country_r4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate"](country_r4);
} }
function UserEditComponent_div_0_Template(rf, ctx) { if (rf & 1) {
    const _r6 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](0, "div", 1)(1, "mat-card", 2)(2, "mat-card-header")(3, "mat-card-title");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](5, "img", 3);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](6, "mat-card-content")(7, "div")(8, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](9, "Location:");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](10, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](12, "div")(13, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](14, "Age:");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](15, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](16);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](17, "div")(18, "strong");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](19, "Member since:");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](20, "p");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](21);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipe"](22, "date");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](23, "div", 4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](24, UserEditComponent_div_0_div_24_Template, 4, 0, "div", 5);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](25, "mat-tab-group", 6)(26, "mat-tab", 7)(27, "form", 8, 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("ngSubmit", function UserEditComponent_div_0_Template_form_ngSubmit_27_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r6); const ctx_r5 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r5.updateUser(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](29, "div", 10)(30, "mat-form-field", 4)(31, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](32, "First name");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](33, "input", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("ngModelChange", function UserEditComponent_div_0_Template_input_ngModelChange_33_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r6); const ctx_r7 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r7.user.firstName = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](34, "mat-form-field", 4)(35, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](36, "Last name");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](37, "input", 12);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("ngModelChange", function UserEditComponent_div_0_Template_input_ngModelChange_37_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r6); const ctx_r8 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r8.user.lastName = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](38, "div", 13)(39, "mat-form-field")(40, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](41, " Bio ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](42, "textarea", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("ngModelChange", function UserEditComponent_div_0_Template_textarea_ngModelChange_42_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r6); const ctx_r9 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r9.user.bio = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](43, "div", 13)(44, "mat-form-field")(45, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](46, " Interested In ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](47, "mat-select", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("ngModelChange", function UserEditComponent_div_0_Template_mat_select_ngModelChange_47_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r6); const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r10.user.interest = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](48, "mat-option", 16);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](49, " Males ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](50, "mat-option", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](51, " Females");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](52, "mat-option", 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](53, " Both ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](54, "div", 13)(55, "mat-form-field", 4)(56, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](57, " Country ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](58, "mat-select", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("ngModelChange", function UserEditComponent_div_0_Template_mat_select_ngModelChange_58_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r6); const ctx_r11 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r11.user.country = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](59, UserEditComponent_div_0_mat_option_59_Template, 2, 2, "mat-option", 20);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](60, "mat-form-field", 4)(61, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](62, " City ");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](63, "input", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("ngModelChange", function UserEditComponent_div_0_Template_input_ngModelChange_63_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵrestoreView"](_r6); const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"](); return ctx_r12.user.city = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](64, "div", 22)(65, "button", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtext"](66, "Save Changes");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementStart"](67, "mat-tab", 24);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelement"](68, "app-photo-editor", 25);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵelementEnd"]()()()();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate2"]("", ctx_r0.user.firstName, " ", ctx_r0.user.lastName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpropertyInterpolate"]("src", (ctx_r0.user.photos[0] == null ? null : ctx_r0.user.photos[0].url) || "./assets/Pictures/profile-placeholder.png", _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵsanitizeUrl"]);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate2"]("", ctx_r0.user.city, ", ", ctx_r0.user.country, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate"](ctx_r0.user.age);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtextInterpolate"](_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpipeBind1"](22, 18, ctx_r0.getLoacaleDateTime(ctx_r0.user.created)));
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", ctx_r0.checkIfUserChanged());
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵpropertyInterpolate1"]("label", "About ", ctx_r0.user.firstName, "");
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](7);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngModel", ctx_r0.user.firstName);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngModel", ctx_r0.user.lastName);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngModel", ctx_r0.user.bio);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](5);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngModel", ctx_r0.user.interest);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](11);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngModel", ctx_r0.user.country);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngForOf", ctx_r0.countryList);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngModel", ctx_r0.user.city);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](2);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("disabled", !ctx_r0.checkIfUserChanged());
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵadvance"](3);
    _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("user", ctx_r0.user);
} }
class UserEditComponent {
    constructor(toastr, userService, accountService) {
        this.toastr = toastr;
        this.userService = userService;
        this.accountService = accountService;
        this.countryList = [
            "Afghanistan",
            "Albania",
            "Algeria",
            "American Samoa",
            "Andorra",
            "Angola",
            "Anguilla",
            "Antarctica",
            "Antigua and Barbuda",
            "Argentina",
            "Armenia",
            "Aruba",
            "Australia",
            "Austria",
            "Azerbaijan",
            "Bahamas (the)",
            "Bahrain",
            "Bangladesh",
            "Barbados",
            "Belarus",
            "Belgium",
            "Belize",
            "Benin",
            "Bermuda",
            "Bhutan",
            "Bolivia (Plurinational State of)",
            "Bonaire, Sint Eustatius and Saba",
            "Bosnia and Herzegovina",
            "Botswana",
            "Bouvet Island",
            "Brazil",
            "British Indian Ocean Territory (the)",
            "Brunei Darussalam",
            "Bulgaria",
            "Burkina Faso",
            "Burundi",
            "Cabo Verde",
            "Cambodia",
            "Cameroon",
            "Canada",
            "Cayman Islands (the)",
            "Central African Republic (the)",
            "Chad",
            "Chile",
            "China",
            "Christmas Island",
            "Cocos (Keeling) Islands (the)",
            "Colombia",
            "Comoros (the)",
            "Congo (the Democratic Republic of the)",
            "Congo (the)",
            "Cook Islands (the)",
            "Costa Rica",
            "Croatia",
            "Cuba",
            "Curaçao",
            "Cyprus",
            "Czechia",
            "Côte d'Ivoire",
            "Denmark",
            "Djibouti",
            "Dominica",
            "Dominican Republic (the)",
            "Ecuador",
            "Egypt",
            "El Salvador",
            "Equatorial Guinea",
            "Eritrea",
            "Estonia",
            "Eswatini",
            "Ethiopia",
            "Falkland Islands (the) [Malvinas]",
            "Faroe Islands (the)",
            "Fiji",
            "Finland",
            "France",
            "French Guiana",
            "French Polynesia",
            "French Southern Territories (the)",
            "Gabon",
            "Gambia (the)",
            "Georgia",
            "Germany",
            "Ghana",
            "Gibraltar",
            "Greece",
            "Greenland",
            "Grenada",
            "Guadeloupe",
            "Guam",
            "Guatemala",
            "Guernsey",
            "Guinea",
            "Guinea-Bissau",
            "Guyana",
            "Haiti",
            "Heard Island and McDonald Islands",
            "Holy See (the)",
            "Honduras",
            "Hong Kong",
            "Hungary",
            "Iceland",
            "India",
            "Indonesia",
            "Iran (Islamic Republic of)",
            "Iraq",
            "Ireland",
            "Isle of Man",
            "Israel",
            "Italy",
            "Jamaica",
            "Japan",
            "Jersey",
            "Jordan",
            "Kazakhstan",
            "Kenya",
            "Kiribati",
            "Korea (the Democratic People's Republic of)",
            "Korea (the Republic of)",
            "Kuwait",
            "Kyrgyzstan",
            "Lao People's Democratic Republic (the)",
            "Latvia",
            "Lebanon",
            "Lesotho",
            "Liberia",
            "Libya",
            "Liechtenstein",
            "Lithuania",
            "Luxembourg",
            "Macao",
            "Madagascar",
            "Malawi",
            "Malaysia",
            "Maldives",
            "Mali",
            "Malta",
            "Marshall Islands (the)",
            "Martinique",
            "Mauritania",
            "Mauritius",
            "Mayotte",
            "Mexico",
            "Micronesia (Federated States of)",
            "Moldova (the Republic of)",
            "Monaco",
            "Mongolia",
            "Montenegro",
            "Montserrat",
            "Morocco",
            "Mozambique",
            "Myanmar",
            "Namibia",
            "Nauru",
            "Nepal",
            "Netherlands (the)",
            "New Caledonia",
            "New Zealand",
            "Nicaragua",
            "Niger (the)",
            "Nigeria",
            "Niue",
            "Norfolk Island",
            "Northern Mariana Islands (the)",
            "Norway",
            "Oman",
            "Pakistan",
            "Palau",
            "Palestine, State of",
            "Panama",
            "Papua New Guinea",
            "Paraguay",
            "Peru",
            "Philippines (the)",
            "Pitcairn",
            "Poland",
            "Portugal",
            "Puerto Rico",
            "Qatar",
            "Republic of North Macedonia",
            "Romania",
            "Russian Federation (the)",
            "Rwanda",
            "Réunion",
            "Saint Barthélemy",
            "Saint Helena, Ascension and Tristan da Cunha",
            "Saint Kitts and Nevis",
            "Saint Lucia",
            "Saint Martin (French part)",
            "Saint Pierre and Miquelon",
            "Saint Vincent and the Grenadines",
            "Samoa",
            "San Marino",
            "Sao Tome and Principe",
            "Saudi Arabia",
            "Senegal",
            "Serbia",
            "Seychelles",
            "Sierra Leone",
            "Singapore",
            "Sint Maarten (Dutch part)",
            "Slovakia",
            "Slovenia",
            "Solomon Islands",
            "Somalia",
            "South Africa",
            "South Georgia and the South Sandwich Islands",
            "South Sudan",
            "Spain",
            "Sri Lanka",
            "Sudan (the)",
            "Suriname",
            "Svalbard and Jan Mayen",
            "Sweden",
            "Switzerland",
            "Syrian Arab Republic",
            "Taiwan",
            "Tajikistan",
            "Tanzania, United Republic of",
            "Thailand",
            "Timor-Leste",
            "Togo",
            "Tokelau",
            "Tonga",
            "Trinidad and Tobago",
            "Tunisia",
            "Turkey",
            "Turkmenistan",
            "Turks and Caicos Islands (the)",
            "Tuvalu",
            "Uganda",
            "Ukraine",
            "United Arab Emirates (the)",
            "United Kingdom of Great Britain and Northern Ireland (the)",
            "United States Minor Outlying Islands (the)",
            "United States of America (the)",
            "Uruguay",
            "Uzbekistan",
            "Vanuatu",
            "Venezuela (Bolivarian Republic of)",
            "Viet Nam",
            "Virgin Islands (British)",
            "Virgin Islands (U.S.)",
            "Wallis and Futuna",
            "Western Sahara",
            "Yemen",
            "Zambia",
            "Zimbabwe",
            "Åland Islands"
        ];
        this.accountService.currentUser$.pipe((0,rxjs__WEBPACK_IMPORTED_MODULE_4__.take)(1)).subscribe(account => {
            if (account) {
                this.account = account;
            }
        });
    }
    unloadNotification($event) {
        if (this.checkIfUserChanged()) {
            $event.returnValue = true;
        }
    }
    ngOnInit() {
        this.loadUser();
    }
    loadUser() {
        var _a, _b;
        if ((_a = this.account) === null || _a === void 0 ? void 0 : _a.userData) {
            this.userService.getUserByUsername((_b = this.account) === null || _b === void 0 ? void 0 : _b.userData.username).subscribe(user => {
                this.user = user;
                this.oldUser = Object.assign({}, this.user);
            });
        }
    }
    checkIfUserChanged() {
        let prop;
        if (this.oldUser && this.user) {
            for (prop in this.oldUser) {
                if (this.oldUser[prop] !== this.user[prop]) {
                    return true;
                }
            }
        }
        return false;
    }
    getLoacaleDateTime(d) {
        var localDate = new Date(d.toString() + 'Z');
        return localDate;
    }
    updateUser() {
        if (this.user) {
            this.userService.updateUser(this.user).subscribe(r => {
                var _a;
                if (r && this.user) {
                    this.toastr.success('Profile updated successfully');
                    this.oldUser = Object.assign({}, this.user);
                    (_a = this.editForm) === null || _a === void 0 ? void 0 : _a.reset(this.user);
                }
            });
        }
    }
}
UserEditComponent.ɵfac = function UserEditComponent_Factory(t) { return new (t || UserEditComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](ngx_toastr__WEBPACK_IMPORTED_MODULE_5__.ToastrService), _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_0__.UserService), _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdirectiveInject"](_services_account_service__WEBPACK_IMPORTED_MODULE_1__.AccountService)); };
UserEditComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵdefineComponent"]({ type: UserEditComponent, selectors: [["app-user-edit"]], viewQuery: function UserEditComponent_Query(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵviewQuery"](_c0, 5);
    } if (rf & 2) {
        let _t;
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵqueryRefresh"](_t = _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵloadQuery"]()) && (ctx.editForm = _t.first);
    } }, hostBindings: function UserEditComponent_HostBindings(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵlistener"]("beforeunload", function UserEditComponent_beforeunload_HostBindingHandler($event) { return ctx.unloadNotification($event); }, false, _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵresolveWindow"]);
    } }, decls: 1, vars: 1, consts: [["class", "row", 4, "ngIf"], [1, "row"], [1, "col-4"], ["mat-card-image", "", "alt", "profile picture", 3, "src"], [1, "col"], ["class", "alert alert-info", 4, "ngIf"], ["mat-align-tabs", "center", 1, "p-2"], [3, "label"], ["id", "editForm", 3, "ngSubmit"], ["editForm", "ngForm"], [1, "row", "mt-4", "mx-0"], ["matInput", "", "name", "firstName", 3, "ngModel", "ngModelChange"], ["matInput", "", "name", "lastName", 3, "ngModel", "ngModelChange"], [1, "row", "mt-2", "mx-0"], ["matInput", "", "name", "bio", 3, "ngModel", "ngModelChange"], ["name", "interest", 3, "ngModel", "ngModelChange"], ["value", "m"], ["value", "f"], ["value", "b"], ["name", "country", 3, "ngModel", "ngModelChange"], [3, "value", 4, "ngFor", "ngForOf"], ["matInput", "", "name", "city", 3, "ngModel", "ngModelChange"], [1, "mt-5", "d-grid", "justify-content-center"], ["mat-flat-button", "", "color", "primary", "form", "editForm", "type", "submit", 3, "disabled"], ["label", "Edit Pictures"], [3, "user"], [1, "alert", "alert-info"], [3, "value"]], template: function UserEditComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵtemplate"](0, UserEditComponent_div_0_Template, 69, 20, "div", 0);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_3__["ɵɵproperty"]("ngIf", ctx.user);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_6__.NgIf, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCard, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardHeader, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardTitle, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardImage, _angular_material_card__WEBPACK_IMPORTED_MODULE_7__.MatCardContent, _angular_material_tabs__WEBPACK_IMPORTED_MODULE_8__.MatTabGroup, _angular_material_tabs__WEBPACK_IMPORTED_MODULE_8__.MatTab, _angular_forms__WEBPACK_IMPORTED_MODULE_9__["ɵNgNoValidate"], _angular_forms__WEBPACK_IMPORTED_MODULE_9__.NgControlStatusGroup, _angular_forms__WEBPACK_IMPORTED_MODULE_9__.NgForm, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_10__.MatFormField, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_10__.MatLabel, _angular_material_input__WEBPACK_IMPORTED_MODULE_11__.MatInput, _angular_forms__WEBPACK_IMPORTED_MODULE_9__.DefaultValueAccessor, _angular_forms__WEBPACK_IMPORTED_MODULE_9__.NgControlStatus, _angular_forms__WEBPACK_IMPORTED_MODULE_9__.NgModel, _angular_material_select__WEBPACK_IMPORTED_MODULE_12__.MatSelect, _angular_material_core__WEBPACK_IMPORTED_MODULE_13__.MatOption, _angular_common__WEBPACK_IMPORTED_MODULE_6__.NgForOf, _angular_material_button__WEBPACK_IMPORTED_MODULE_14__.MatButton, _photo_editor_photo_editor_component__WEBPACK_IMPORTED_MODULE_2__.PhotoEditorComponent], pipes: [_angular_common__WEBPACK_IMPORTED_MODULE_6__.DatePipe], styles: [".img-thumbnail[_ngcontent-%COMP%] {\r\n  margin: 25px;\r\n  width: 85%;\r\n  height: 85%;\r\n}\r\n\r\n.card-body[_ngcontent-%COMP%] {\r\n  padding: 0 25px;\r\n}\r\n\r\n.card-footer[_ngcontent-%COMP%] {\r\n  padding: 10px 15px;\r\n  background-color: #fff;\r\n  border-top: none;\r\n}\r\n\r\n.ngx-gallery[_ngcontent-%COMP%] {\r\n  display: inline-block;\r\n  margin-bottom: 20px;\r\n}\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInVzZXItZWRpdC5jb21wb25lbnQuY3NzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0VBQ0UsWUFBWTtFQUNaLFVBQVU7RUFDVixXQUFXO0FBQ2I7O0FBRUE7RUFDRSxlQUFlO0FBQ2pCOztBQUVBO0VBQ0Usa0JBQWtCO0VBQ2xCLHNCQUFzQjtFQUN0QixnQkFBZ0I7QUFDbEI7O0FBRUE7RUFDRSxxQkFBcUI7RUFDckIsbUJBQW1CO0FBQ3JCIiwiZmlsZSI6InVzZXItZWRpdC5jb21wb25lbnQuY3NzIiwic291cmNlc0NvbnRlbnQiOlsiLmltZy10aHVtYm5haWwge1xyXG4gIG1hcmdpbjogMjVweDtcclxuICB3aWR0aDogODUlO1xyXG4gIGhlaWdodDogODUlO1xyXG59XHJcblxyXG4uY2FyZC1ib2R5IHtcclxuICBwYWRkaW5nOiAwIDI1cHg7XHJcbn1cclxuXHJcbi5jYXJkLWZvb3RlciB7XHJcbiAgcGFkZGluZzogMTBweCAxNXB4O1xyXG4gIGJhY2tncm91bmQtY29sb3I6ICNmZmY7XHJcbiAgYm9yZGVyLXRvcDogbm9uZTtcclxufVxyXG5cclxuLm5neC1nYWxsZXJ5IHtcclxuICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XHJcbiAgbWFyZ2luLWJvdHRvbTogMjBweDtcclxufSJdfQ== */"] });


/***/ }),

/***/ 1771:
/*!********************************************************!*\
  !*** ./src/app/users/user-list/user-list.component.ts ***!
  \********************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "UserListComponent": () => (/* binding */ UserListComponent)
/* harmony export */ });
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _services_user_service__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../../_services/user.service */ 5089);
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/common */ 6362);
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/forms */ 587);
/* harmony import */ var _angular_material_form_field__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! @angular/material/form-field */ 9076);
/* harmony import */ var _angular_material_input__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/material/input */ 3365);
/* harmony import */ var _angular_material_select__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! @angular/material/select */ 1434);
/* harmony import */ var _angular_material_core__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(/*! @angular/material/core */ 8133);
/* harmony import */ var _angular_material_button__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(/*! @angular/material/button */ 7317);
/* harmony import */ var _user_card_user_card_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../user-card/user-card.component */ 9412);
/* harmony import */ var _angular_material_paginator__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(/*! @angular/material/paginator */ 6439);











function UserListComponent_form_1_mat_error_7_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, "18 is the minimum");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} }
function UserListComponent_form_1_mat_error_12_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-error");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1, "18 is the minimum");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} }
function UserListComponent_form_1_mat_option_17_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-option", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const gender_r8 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("value", gender_r8.value);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](gender_r8.display);
} }
function UserListComponent_form_1_mat_option_22_Template(rf, ctx) { if (rf & 1) {
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "mat-option", 19);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
} if (rf & 2) {
    const oreder_r9 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("value", oreder_r9.value);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtextInterpolate"](oreder_r9.display);
} }
function UserListComponent_form_1_Template(rf, ctx) { if (rf & 1) {
    const _r11 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "form", 5, 6);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngSubmit", function UserListComponent_form_1_Template_form_ngSubmit_0_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r11); const ctx_r10 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r10.loadUsers(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](2, "div", 7)(3, "mat-form-field", 8)(4, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](5, "Age from");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](6, "input", 9);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngModelChange", function UserListComponent_form_1_Template_input_ngModelChange_6_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r11); const ctx_r12 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r12.userParams.minAge = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](7, UserListComponent_form_1_mat_error_7_Template, 2, 0, "mat-error", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](8, "mat-form-field", 8)(9, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](10, "Age to");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](11, "input", 11);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngModelChange", function UserListComponent_form_1_Template_input_ngModelChange_11_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r11); const ctx_r13 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r13.userParams.maxAge = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](12, UserListComponent_form_1_mat_error_12_Template, 2, 0, "mat-error", 10);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](13, "mat-form-field", 12)(14, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](15, "Show");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](16, "mat-select", 13);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngModelChange", function UserListComponent_form_1_Template_mat_select_ngModelChange_16_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r11); const ctx_r14 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r14.userParams.sex = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](17, UserListComponent_form_1_mat_option_17_Template, 2, 2, "mat-option", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](18, "mat-form-field", 12)(19, "mat-label");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](20, "Order By");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](21, "mat-select", 15);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("ngModelChange", function UserListComponent_form_1_Template_mat_select_ngModelChange_21_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r11); const ctx_r15 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r15.userParams.orderBy = $event; });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](22, UserListComponent_form_1_mat_option_22_Template, 2, 2, "mat-option", 14);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](23, "div", 16)(24, "button", 17);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](25, "Apply");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](26, "button", 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("click", function UserListComponent_form_1_Template_button_click_26_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r11); const ctx_r16 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r16.resetFilters(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtext"](27, "Reset");
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()()();
} if (rf & 2) {
    const ctx_r0 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](6);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngModel", ctx_r0.userParams.minAge);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r0.userParams.minAge < 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngModel", ctx_r0.userParams.maxAge);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx_r0.userParams.maxAge && ctx_r0.userParams.maxAge < 18);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngModel", ctx_r0.userParams.sex);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngForOf", ctx_r0.genderList);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](4);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngModel", ctx_r0.userParams.orderBy);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngForOf", ctx_r0.orderByOptions);
} }
function UserListComponent_div_3_Template(rf, ctx) { if (rf & 1) {
    const _r19 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 20)(1, "app-user-card", 21);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("liked", function UserListComponent_div_3_Template_app_user_card_liked_1_listener() { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r19); const ctx_r18 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r18.loadUsers(); });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
} if (rf & 2) {
    const user_r17 = ctx.$implicit;
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("user", user_r17);
} }
function UserListComponent_div_4_Template(rf, ctx) { if (rf & 1) {
    const _r21 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵgetCurrentView"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 22)(1, "mat-paginator", 23);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵlistener"]("page", function UserListComponent_div_4_Template_mat_paginator_page_1_listener($event) { _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵrestoreView"](_r21); const ctx_r20 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"](); return ctx_r20.pageChanged($event); });
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]()();
} if (rf & 2) {
    const ctx_r2 = _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵnextContext"]();
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
    _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("length", ctx_r2.pagination.totalItems)("pageSize", ctx_r2.pagination.itemsPerPage);
} }
class UserListComponent {
    constructor(userService) {
        this.userService = userService;
        this.users = [];
        this.pagination = null;
        this.userParams = null;
        this.genderList = [{ value: 'm', display: 'Males' }, { value: 'f', display: 'Females' }, { value: 'b', display: 'Both' }];
        this.orderByOptions = [{ value: 'lastActive', display: 'Last Active' }, { value: 'creationTime', display: 'Newest Users' }, { value: 'age', display: 'Age' }];
        this.userParams = userService.getUserParams();
    }
    ngOnInit() {
        this.loadUsers();
    }
    loadUsers() {
        this.userService.setUserParams(this.userParams);
        this.userService.getAllUsers(this.userParams).subscribe(response => {
            if (response) {
                this.users = response.result;
                this.pagination = response.pagination;
            }
        });
    }
    resetFilters() {
        this.userParams = this.userService.resetUserParams();
        this.loadUsers();
    }
    pageChanged($event) {
        if ($event) {
            if ($event && this.userParams) {
                this.userParams.pageNumber = $event.pageIndex + 1;
                this.userService.setUserParams(this.userParams);
                this.loadUsers();
            }
        }
    }
}
UserListComponent.ɵfac = function UserListComponent_Factory(t) { return new (t || UserListComponent)(_angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdirectiveInject"](_services_user_service__WEBPACK_IMPORTED_MODULE_0__.UserService)); };
UserListComponent.ɵcmp = /*@__PURE__*/ _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵdefineComponent"]({ type: UserListComponent, selectors: [["app-member-list"]], decls: 5, vars: 3, consts: [[1, "container", "mt-3"], ["class", "", 3, "ngSubmit", 4, "ngIf"], [1, "row", "mt-4"], ["class", "col-3", 4, "ngFor", "ngForOf"], ["class", "d-flex justify-content-center", 4, "ngIf"], [1, "", 3, "ngSubmit"], ["form", "ngForm"], [1, "row", "justify-content-center"], [1, "col-1"], ["matInput", "", "type", "number", "name", "minAge", "min", "18", 3, "ngModel", "ngModelChange"], [4, "ngIf"], ["matInput", "", "type", "number", "name", "maxAge", "min", "18", 3, "ngModel", "ngModelChange"], [1, "col-2"], ["name", "sex", 3, "ngModel", "ngModelChange"], [3, "value", 4, "ngFor", "ngForOf"], ["name", "orderBy", 3, "ngModel", "ngModelChange"], [1, "mt-4", "row", "justify-content-center"], ["mat-raised-button", "", "color", "primary", "type", "submit", 1, "col-auto", "me-1"], ["mat-raised-button", "", "color", "accent", "type", "submit", 1, "col-auto", 3, "click"], [3, "value"], [1, "col-3"], [3, "user", "liked"], [1, "d-flex", "justify-content-center"], ["aria-label", "Select page", 3, "length", "pageSize", "page"]], template: function UserListComponent_Template(rf, ctx) { if (rf & 1) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](0, "div", 0);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](1, UserListComponent_form_1_Template, 28, 8, "form", 1);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementStart"](2, "div", 2);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](3, UserListComponent_div_3_Template, 2, 1, "div", 3);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵelementEnd"]();
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵtemplate"](4, UserListComponent_div_4_Template, 2, 2, "div", 4);
    } if (rf & 2) {
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.userParams);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](2);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngForOf", ctx.users);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵadvance"](1);
        _angular_core__WEBPACK_IMPORTED_MODULE_2__["ɵɵproperty"]("ngIf", ctx.userParams && ctx.pagination && ctx.pagination.totalPages > 1);
    } }, directives: [_angular_common__WEBPACK_IMPORTED_MODULE_3__.NgIf, _angular_forms__WEBPACK_IMPORTED_MODULE_4__["ɵNgNoValidate"], _angular_forms__WEBPACK_IMPORTED_MODULE_4__.NgControlStatusGroup, _angular_forms__WEBPACK_IMPORTED_MODULE_4__.NgForm, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_5__.MatFormField, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_5__.MatLabel, _angular_material_input__WEBPACK_IMPORTED_MODULE_6__.MatInput, _angular_forms__WEBPACK_IMPORTED_MODULE_4__.MinValidator, _angular_forms__WEBPACK_IMPORTED_MODULE_4__.NumberValueAccessor, _angular_forms__WEBPACK_IMPORTED_MODULE_4__.DefaultValueAccessor, _angular_forms__WEBPACK_IMPORTED_MODULE_4__.NgControlStatus, _angular_forms__WEBPACK_IMPORTED_MODULE_4__.NgModel, _angular_material_form_field__WEBPACK_IMPORTED_MODULE_5__.MatError, _angular_material_select__WEBPACK_IMPORTED_MODULE_7__.MatSelect, _angular_common__WEBPACK_IMPORTED_MODULE_3__.NgForOf, _angular_material_core__WEBPACK_IMPORTED_MODULE_8__.MatOption, _angular_material_button__WEBPACK_IMPORTED_MODULE_9__.MatButton, _user_card_user_card_component__WEBPACK_IMPORTED_MODULE_1__.UserCardComponent, _angular_material_paginator__WEBPACK_IMPORTED_MODULE_10__.MatPaginator], styles: ["\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJ1c2VyLWxpc3QuY29tcG9uZW50LmNzcyJ9 */"] });


/***/ }),

/***/ 2340:
/*!*****************************************!*\
  !*** ./src/environments/environment.ts ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   "environment": () => (/* binding */ environment)
/* harmony export */ });
const environment = {
    production: false,
    apiUrl: 'https://localhost:5001/api/',
    hubUrl: 'https://localhost:5001/hubs/'
};


/***/ }),

/***/ 4431:
/*!*********************!*\
  !*** ./src/main.ts ***!
  \*********************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _angular_platform_browser__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! @angular/platform-browser */ 318);
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/core */ 3184);
/* harmony import */ var _app_app_module__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./app/app.module */ 6747);
/* harmony import */ var _environments_environment__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./environments/environment */ 2340);




if (_environments_environment__WEBPACK_IMPORTED_MODULE_1__.environment.production) {
    (0,_angular_core__WEBPACK_IMPORTED_MODULE_2__.enableProdMode)();
}
_angular_platform_browser__WEBPACK_IMPORTED_MODULE_3__.platformBrowser().bootstrapModule(_app_app_module__WEBPACK_IMPORTED_MODULE_0__.AppModule)
    .catch(err => console.error(err));


/***/ })

},
/******/ __webpack_require__ => { // webpackRuntimeModules
/******/ var __webpack_exec__ = (moduleId) => (__webpack_require__(__webpack_require__.s = moduleId))
/******/ __webpack_require__.O(0, ["vendor"], () => (__webpack_exec__(4431)));
/******/ var __webpack_exports__ = __webpack_require__.O();
/******/ }
]);
//# sourceMappingURL=main.js.map