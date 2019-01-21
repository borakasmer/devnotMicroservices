import { Component, OnInit } from '@angular/core';
import { ShoppingModel } from 'src/Models/ShoppingModel';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'shoppingAdmin';
  dataList: ShoppingModel[] = [];

  public ngOnInit() {
    let hubConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:1923/productsign")
      .build();

    hubConnection.start().then(res => {
      console.log("HubConnection Start");
    });

    hubConnection.on("GetConnectionID", (connectionID: string) => {
      console.log(`ConnectionID : ${connectionID}`);
    });

    hubConnection.on("GetProduct", (product: ShoppingModel) => {
      this.dataList.push(product);
    });
  }

}
