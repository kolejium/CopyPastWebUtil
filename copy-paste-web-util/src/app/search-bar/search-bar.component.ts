import { Component, OnInit } from '@angular/core';
import { SearchBarStore } from '../core/stores/search-bar.store';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss']
})
export class SearchBarComponent implements OnInit {
  value!: string;

  constructor(private searchBarStore: SearchBarStore) { }


  onEnter() {
    this.searchBarStore.setQuery(this.value);
  }
  
  onReset() {
    this.value = '';
  }

  ngOnInit(): void {
  }

}
