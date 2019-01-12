//import { COMMA, ENTER } from '@angular/cdk/keycodes';
//import { Component } from '@angular/core';
//import { MatChipInputEvent } from '@angular/material';



///**
// * @title Chips with input
// */
//@Component({
//  selector: 'app-nav-menu',
//  templateUrl: './nav-menu.component.html',
//  styleUrls: ['./nav-menu.component.css']
//})

//export class Tag {
//  name: string;
//}

//import { COMMA, ENTER } from '@angular/cdk/keycodes';
//import { Component } from '@angular/core';
//import { MatChipInputEvent } from '@angular/material';

//export interface Fruit {
//  name: string;
//}

///**
// * @title Chips with input
// */
//@Component({
//  selector: 'app-nav-menu',
//  templateUrl: './nav-menu.component.html',
//  styleUrls: ['./nav-menu.component.css'],
//})
//export class ChipsInputExample {
//  visible = true;
//  selectable = true;
//  removable = true;
//  addOnBlur = true;
//  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
//  fruits: Fruit[] = [
//    { name: 'Lemon' },
//    { name: 'Lime' },
//    { name: 'Apple' },
//  ];

//  add(event: MatChipInputEvent): void {
//    const input = event.input;
//    const value = event.value;

//    // Add our fruit
//    if ((value || '').trim()) {
//      this.fruits.push({ name: value.trim() });
//    }

//    // Reset the input value
//    if (input) {
//      input.value = '';
//    }
//  }

//  remove(fruit: Fruit): void {
//    const index = this.fruits.indexOf(fruit);

//    if (index >= 0) {
//      this.fruits.splice(index, 1);
//    }
//  }
//}
