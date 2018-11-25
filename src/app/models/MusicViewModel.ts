export class MusicViewModel {
    public path: string;

    public static fromFile(file){
        let item = new MusicViewModel();
        item.path = file.name;
        return item;
    }
}