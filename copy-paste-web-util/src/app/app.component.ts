import { Component, OnInit, SecurityContext } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { ApiService } from './core/services/api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  source!: SafeResourceUrl;
  title = 'copy-paste-web-util';

  constructor(private apiService: ApiService, private domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
  }

  openSource(value: string) {
    //this.source = this.domSanitizer.bypassSecurityTrustResourceUrl(this.apiService.proxySiteByUrl(value));

    this.source = this.domSanitizer.bypassSecurityTrustResourceUrl(this.apiService.getUrlForGetSiteByUrl(value));
    //this.apiService.getSiteByUrl(value).subscribe((z : string) => this.source = this.domSanitizer.bypassSecurityTrustResourceUrl(z));
  }
}
