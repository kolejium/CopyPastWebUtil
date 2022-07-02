import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-demo-view',
  templateUrl: './demo-view.component.html',
  styleUrls: ['./demo-view.component.scss']
})
export class DemoViewComponent implements OnInit {
  @Input() value: string | undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
