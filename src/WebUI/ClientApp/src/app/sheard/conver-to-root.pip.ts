import { Pipe, PipeTransform } from "@angular/core";

@Pipe(

    {
        name: 'converToRoot'
    }
)

export class converToRoot implements PipeTransform {

    transform(value: any) {
        
        return "https://localhost:44447/api/Images/"+value
        //return value.replace("wwwroot",root);
    }

}