import { Inject, Injectable, inject } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { environment } from '@environments/environment';
import { DOCUMENT } from '@angular/common';

export interface MetaData {
  title: string | undefined;
  description: string | undefined;
  url: string | undefined;
  tags: string[] | undefined;
}

@Injectable({
  providedIn: 'root',
})
export class UiService {
  private meta = inject(Meta);
  private title = inject(Title);

  constructor(@Inject(DOCUMENT) private doc: Document) {
    let link: HTMLLinkElement = doc.createElement('link');
    link.setAttribute('rel', 'canonical');
    link.setAttribute('id', 'canonical');
    doc.head.appendChild(link);
    link.setAttribute('href', doc.URL); 
  }

  setMeta(metadata: MetaData) {
    this.title.setTitle(metadata.title != undefined ? `${environment.sitename} - ${metadata.title}` : environment.sitename);
    this.meta.updateTag({ name: 'og:title', content: metadata.title != undefined ? `${environment.sitename} - ${metadata.title}` : environment.sitename });
    const keywords =
      metadata.tags && metadata.tags.length != 0
        ? metadata.tags.join(', ')
        : 'dotnet, angular, c#, developer, development, docker, Microsoft, Microsoft SQL Server, python, vue';
    const description =
      metadata.description != undefined ? metadata.description : `${environment.sitename} provides development of applications`;
    this.meta.updateTag({ name: 'og:url', content: environment.site + metadata.url });
    this.meta.updateTag({ name: 'twitter:url', content: environment.site + metadata.url });
    this.meta.updateTag({ name: 'description', content: description });
    this.meta.updateTag({ name: 'og:description', content: description });
    this.meta.updateTag({ name: 'twitter:description', content: description });
    this.meta.updateTag({ name: 'keywords', content: keywords });
    this.meta.updateTag({ name: 'og:site_name', content: `${environment.sitename} - Application development` });
    this.meta.updateTag({ name: 'og:type', content: 'website' });
    this.meta.updateTag({ name: 'Generator', content: `${environment.sitename} - Content Management System` });

    let link = this.doc.getElementById('canonical');
    if(link){
      link.setAttribute('rel', 'canonical');
      link.setAttribute('href', environment.site + metadata.url); 
    }
  }
}
