import { Component } from '@angular/core';
import { ShoppingModel } from 'src/Models/ShoppingModel';
import { ShoppingService } from 'src/Services/ShoppingService';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'shoppingClient';
  data: ShoppingModel;
  dataList: ShoppingModel[] = [];

  constructor(private service: ShoppingService) {
    this.data = new ShoppingModel();
  }

  public save() {
    this.service.PushProduct(this.data);
    this.dataList.push(this.data);
    this.data = new ShoppingModel();
  }
}
