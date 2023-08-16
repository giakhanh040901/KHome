export class TradingProvider {
    tradingProviderId: number;
    labelName: string;
  
    constructor(tradingProviderId: number, labelName: string) {
      this.tradingProviderId = tradingProviderId;
      this.labelName = labelName;
    }
}