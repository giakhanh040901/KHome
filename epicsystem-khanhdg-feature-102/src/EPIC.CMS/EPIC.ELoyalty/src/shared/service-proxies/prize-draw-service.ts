import { Injectable } from "@angular/core";
import { CreateOrEditPrizeDraw } from "@shared/interface/prize-draw-management/PrizeDrawManagement.model";
import { Subject } from "rxjs";

@Injectable()
export class PrizeDrawShareService{
    private prizeDrawComplete = new Subject<any>();
    prizeDrawComplete$ = this.prizeDrawComplete.asObservable();

	prizeDrawProgramInfo: CreateOrEditPrizeDraw = new CreateOrEditPrizeDraw(); 

    filterProgramInfo: boolean;
    filterProgramConfig: boolean;

    reset(){
	    this.prizeDrawProgramInfo = new CreateOrEditPrizeDraw(); 
    }

    getPrizeDrawProgramInfo(){
        return this.prizeDrawProgramInfo;
    }

    setPrizeDrawProgramInfo(prizeDrawProgramInfo: CreateOrEditPrizeDraw){
        this.prizeDrawProgramInfo = prizeDrawProgramInfo;
    }

    complete() {
        this.prizeDrawComplete.next(this.prizeDrawProgramInfo);
    }
}
