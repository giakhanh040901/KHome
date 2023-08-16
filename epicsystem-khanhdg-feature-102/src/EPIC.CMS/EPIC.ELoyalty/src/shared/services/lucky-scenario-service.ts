import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, Optional } from "@angular/core";
import { API_BASE_URL, ServiceProxyBase } from "@shared/service-proxies/service-proxies-base";
import { CookieService } from "ngx-cookie-service";
import { MessageService } from "primeng/api";
import { Observable } from "rxjs";

@Injectable()
export class LuckyScenarioService extends ServiceProxyBase {
    private readonly baseAPI = '/api/loyalty/lucky-scenario';

    constructor(
        messageService: MessageService, 
        _cookieService: CookieService, 
        @Inject(HttpClient) http: HttpClient, 
        @Optional() @Inject(API_BASE_URL) baseUrl?: string
        ) {
        super(messageService, _cookieService, http, baseUrl);
    }

    findById(id: number): Observable<any>{
        return this.requestGet(`${this.baseAPI}/find-by-id/${id}`);
    }

    changeStatus(id: number): Observable<any>{
        return this.requestPut(null, `${this.baseAPI}/change-status/${id}`);
    }

    delete(id: number): Observable<any>{
        return this.requestPut(null, `${this.baseAPI}/delete/${id}`);
    }

    addScenario(body): Observable<any>{
        let formData: FormData = new FormData();
        Object.keys(body).forEach((key) => {
            if(key === 'avatarImageUrl' && body[key] instanceof File) {
                formData.append('avatarImageUrl', body[key], body[key].name);
            } else if(key === 'luckyScenarioDetails'){
                for(let i=0; i < body['luckyScenarioDetails'].length ; i++) {
                    let scenarioDetail = body['luckyScenarioDetails'][i];
                    if (scenarioDetail.voucherId < 0) delete scenarioDetail.voucherId;
                    Object.keys(scenarioDetail).forEach((scenarioProperty) => {
                        if(scenarioDetail[scenarioProperty]) formData.append(`luckyScenarioDetails[${i}].${scenarioProperty}`, scenarioDetail[scenarioProperty]);
                    })
                }
            } else if(key === 'luckyRotationInterface'){
                const luckySpinSetting = body['luckyRotationInterface'];
                Object.keys(body['luckyRotationInterface']).forEach(luckySpinProperty => {
                    const luckySpinValue = luckySpinSetting[luckySpinProperty];
                    if(luckySpinValue instanceof File) {
                        formData.append(`luckyRotationInterface.${luckySpinProperty}`, luckySpinValue, luckySpinValue?.name);
                    } else {
                        if(luckySpinValue) formData.append(`luckyRotationInterface.${luckySpinProperty}`, luckySpinValue)
                    }
                });
            }
            else {
                if(body[key]) formData.append(key, body[key]);
            }
        })
        return this.requestPostFormData(formData, `${this.baseAPI}/add`);
    }

    updateScenario(body): Observable<any>{
        let formData: FormData = new FormData();
        Object.keys(body).forEach((key) => {
            if(key === 'avatarImageUrl' && body[key] instanceof File) {
                formData.append('avatarImageUrl', body[key], body[key].name);
            } else if(key === 'luckyScenarioDetails'){
                for(let i=0; i < body[key].length; i++){
                    let luckyScenario = body[key][i];
                    if (luckyScenario.voucherId < 0) delete luckyScenario.voucherId;
                    Object.keys(luckyScenario).forEach((luckyScenarioProperty) => {
                        if(luckyScenario[luckyScenarioProperty]) formData.append(`LuckyScenarioDetails[${i}].${luckyScenarioProperty}`, luckyScenario[luckyScenarioProperty]);
                    })
                }
            } else {
                if(body[key]) formData.append(key, body[key]);
            }
        })
        return this.requestPutFormData(formData, `${this.baseAPI}/update`);
    }

    updateRotationInterface(body): Observable<any>{
        let formData: FormData = new FormData();
        Object.keys(body).forEach((key) => {
            const luckySpinValue = body[key];
            if(luckySpinValue instanceof File) {
                formData.append(key, luckySpinValue, luckySpinValue?.name);
            } else {
                if(luckySpinValue) formData.append(key , luckySpinValue)
            }
        })
        return this.requestPutFormData(formData, `${this.baseAPI}/rotation-interface/update?luckyScenarioId=${body.luckyScenarioId}`);
    }
}