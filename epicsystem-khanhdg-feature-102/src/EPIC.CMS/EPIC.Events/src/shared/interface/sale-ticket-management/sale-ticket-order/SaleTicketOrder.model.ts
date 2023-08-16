export class SaleTicketOrderModel {
  public id: number;
  public requestCode: string;
  public customerName: string;
  public customerPhone: string;
  public event: string;
  public ticketQuantity: number;
  public value: string;
  public orderDate: string;
  public type: number;
  public source: number;
  public restTime: number | undefined;
  public isHavePayment: boolean;
  public status: number;
}
