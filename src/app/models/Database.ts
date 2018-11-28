export interface IModel {
    type: string;
}

export class PathModel implements IModel {
    public type = "path";

    public constructor(
        public path: string
    ) {
    }
}

export class MusicMetaDataModel implements IModel {
    public type = "music";

    public artists: string[];
    public album: string;
    public albumArtists: string[];
    public title: string;
    public year: string;
    public track: { no: number, of: number } | null;
    public disk: { no: number, of: number } | null;
    public genres: string[];
    public picture: any;
    public duration: number;

    public file: string;
}