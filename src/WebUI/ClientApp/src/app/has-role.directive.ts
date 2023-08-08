import { Directive, Input } from '@angular/core';
import { TemplateRef, ViewContainerRef } from "@angular/core";
import { UsersClient } from './web-api-client';
@Directive({
  selector: '[userHasRole]'
})
export class HasRoleDirective {

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private usersClinet: UsersClient
  ) { }

  @Input("userHasRole") set hasRole(roleType: string) {
    this.usersClinet.get(localStorage.getItem('username')).subscribe(
      {
        next: user => {
          console.log(user.role)
          if (user.role == roleType) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          }
          else {
            this.viewContainer.clear();
          }
        }
      }
    )
  }

}
