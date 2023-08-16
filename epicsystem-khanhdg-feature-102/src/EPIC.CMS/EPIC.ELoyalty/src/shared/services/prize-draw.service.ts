import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { PrizeDrawManagement } from "@shared/AppConsts";
import { Page } from "@shared/model/page";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class PrizeDrawService extends ServiceProxyBase {
    private readonly baseAPI = '/api/loyalty/lucky-program';
    private readonly baseAPIScenario = '/api/loyalty/lucky-scenario';
    
    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    create(body): Observable<any>{
        let formData: FormData = new FormData();
        Object.keys(body).forEach((key) => {
            if(key === 'avatarImageUrl' && body[key] instanceof File) {
                formData.append('avatarImageUrl', body[key], body[key].name);
            } else if(key === 'luckyScenarios') {
                for(let i=0; i < body[key].length; i++){
                    let luckyScenario = body[key][i];
                    Object.keys(luckyScenario).forEach((luckyScenarioProperty) => {
                        if (luckyScenarioProperty === 'avatarImageUrl' && luckyScenario[luckyScenarioProperty] instanceof File){
                            formData.append(`luckyScenarios[${i}].avatarImageUrl`, luckyScenario[luckyScenarioProperty], luckyScenario[luckyScenarioProperty]?.name);
                        } else 
                        if(luckyScenarioProperty === 'luckyRotationInterface') {
                            const luckySpinSetting = luckyScenario['luckyRotationInterface'];
                            Object.keys(luckyScenario['luckyRotationInterface']).forEach(luckySpinProperty => {
                                const luckySpinValue = luckySpinSetting[luckySpinProperty];
                                if(luckySpinValue instanceof File) {
                                    formData.append(`luckyScenarios[${i}].luckyRotationInterface.${luckySpinProperty}`, luckySpinValue, luckySpinValue?.name);
                                } else {
                                    if(luckySpinValue) formData.append(`luckyScenarios[${i}].luckyRotationInterface.${luckySpinProperty}`, luckySpinValue)
                                }
                            });
                        } else
                        if(luckyScenarioProperty === 'luckyScenarioDetails') {
                            for(let j=0; j < luckyScenario['luckyScenarioDetails'].length ; j++) {
                                let scenarioDetail = luckyScenario['luckyScenarioDetails'][j];
                                if (scenarioDetail.voucherId < 0) delete scenarioDetail.voucherId;
                                Object.keys(scenarioDetail).forEach((scenarioProperty) => {
                                    if(scenarioDetail[scenarioProperty]) formData.append(`luckyScenarios[${i}].luckyScenarioDetails[${j}].${scenarioProperty}`, scenarioDetail[scenarioProperty]);
                                })
                            }
                        } else {
                            if(luckyScenario[luckyScenarioProperty]) formData.append(`luckyScenarios[${i}].${luckyScenarioProperty}`, luckyScenario[luckyScenarioProperty]);
                        }
                    })
                }
            } else {
                if(body[key]) formData.append(key, body[key]);
            }
        })
        return this.requestPostFormData(formData, `${this.baseAPI}/add`);
    }

    update(body): Observable<any>{
        let formData: FormData = new FormData();
        Object.keys(body).forEach((key) => {
            if(key === 'avatarImageUrl' && body[key] instanceof File) {
                formData.append('avatarImageUrl', body[key], body[key].name);
            } else {
                if(body[key]) formData.append(key, body[key]);
            }
        })
        return this.requestPutFormData(formData, `${this.baseAPI}/update`)
    }

    getAll(page: Page, filter: any): Observable<any>{
        let url_ = this.baseAPI + '/find-all?';
        url_ += this.convertParamUrl('pageNumber', page.getPageNumber());
        url_ += this.convertParamUrl('pageSize', page.pageSize);
        if(filter.keyword) url_ += this.convertParamUrl('keyword', filter.keyword);
        if(filter.startDate) url_ += this.convertParamUrl('startDate', filter.startDate);
        if(filter.endDate) url_ += this.convertParamUrl('endDate', filter.endDate);
        if(filter.status) {
            filter.status == PrizeDrawManagement.HET_HAN 
                ? url_ += this.convertParamUrl('isExpried', true)
                : url_ += this.convertParamUrl('status', filter.status);
        } 
        
        return this.requestGet(url_);
    }

    getById(id: number): Observable<any>{
        return this.requestGet(`${this.baseAPI}/find-by-id/${id}`);
    }

    cancel(luckyProgramId): Observable<any>{
        return this.requestPut(null, `${this.baseAPI}/change-status/${luckyProgramId}`);
    }

    delete(luckyProgramId): Observable<any>{
        return this.requestPut(null, `${this.baseAPI}/delete/${luckyProgramId}`);
    }


    // 
    // API kịch bản
    // 
    getAllLuckyScenario(id: number): Observable<any>{
        return this.requestGet(`${this.baseAPIScenario}/get-all?luckyProgramId=${id}`);
    }

}