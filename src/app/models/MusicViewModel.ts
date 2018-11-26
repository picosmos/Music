export class MusicViewModel {
    public path: string;
    private file: any;

    public static fromFile(file) {
        let item = new MusicViewModel();
        item.path = file.name;
        item.file = file;
        return item;
    }
} 