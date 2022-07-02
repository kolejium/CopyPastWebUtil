import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss']
})
export class SearchBarComponent implements OnInit {
  value: string | undefined;
  @Output() queryEvent = new EventEmitter<string>();

  constructor() { }


  onEnter() {
    this.queryEvent.emit(this.value);
  }
  
  onReset() { 
    this.value = undefined;
    this.queryEvent.emit(this.value);
  }

  ngOnInit(): void {
  }

}
