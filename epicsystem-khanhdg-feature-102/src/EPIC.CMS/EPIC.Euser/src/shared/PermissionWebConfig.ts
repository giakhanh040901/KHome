import { PermissionTypes, WebKeys } from './AppConsts';

const PermissionWebConfig = {};

export class PermissionWebConst {
        private static readonly Web: string = "web_";
        public static readonly CoreModule: string = "core.";
        public static readonly BondModule: string = "bond.";
        public static readonly InvestModule: string = "invest.";
        public static readonly SupportModule: string = "support.";
        public static readonly GarnerModule: string = "garner.";
        public static readonly RealStateModule: string = "real_state.";
        public static readonly LoyaltyModule: string = "loyalty.";
        public static readonly EventModule: string = "event.";
        public static readonly UserModule: string = "user.";

        public static readonly CoreWebsite: string = `${PermissionWebConst.CoreModule}${PermissionWebConst.Web}`;
        public static readonly BondWebsite: string = `${PermissionWebConst.BondModule}${PermissionWebConst.Web}`;
        public static readonly InvestWebsite: string = `${PermissionWebConst.InvestModule}${PermissionWebConst.Web}`;
        public static readonly SupportWebsite: string = `${PermissionWebConst.SupportModule}${PermissionWebConst.Web}`;
        public static readonly GarnerWebsite: string = `${PermissionWebConst.GarnerModule}${PermissionWebConst.Web}`;
        public static readonly RealStateWebsite: string = `${PermissionWebConst.RealStateModule}${PermissionWebConst.Web}`;
        public static readonly LoyaltyWebsite: string = `${PermissionWebConst.LoyaltyModule}${PermissionWebConst.Web}`;
        public static readonly EventWebsite: string = `${PermissionWebConst.EventModule}${PermissionWebConst.Web}`;
        //
        public static readonly UserWebsite: string = `${PermissionWebConst.UserModule}${PermissionWebConst.Web}`;
}

PermissionWebConfig[PermissionWebConst.UserWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.User,
        name: 'EPIC-User',
        url: 'https://euser-staging.epicpartner.vn',
        description: 'Web.User',
        createdDate: '08-06-2022',
};

PermissionWebConfig[PermissionWebConst.CoreWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.Core,
        name: 'EPIC-Core',
        url: 'https://ecore-staging.epicpartner.vn',
        description: 'Web.Core',
        createdDate: '08-06-2022',
};

PermissionWebConfig[PermissionWebConst.BondWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.Bond,
        name: 'EPIC-Bond',
        url: 'https://ebond-staging.epicpartner.vn',
        description: 'Web.Bond',
        createdDate: '08-06-2022',
};

PermissionWebConfig[PermissionWebConst.InvestWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.Invest,
        name: 'EPIC-Invest',
        url: 'https://einvest-staging.epicpartner.vn',
        description: 'Web.Invest',
        createdDate: '08-06-2022',
};

PermissionWebConfig[PermissionWebConst.SupportWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.Support,
        name: 'EPIC-Support',
        url: 'https://esupport-staging.epicpartner.vn',
        description: 'Web.Invest',
        createdDate: '08-06-2022',
};

PermissionWebConfig[PermissionWebConst.GarnerWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.Garner,
        name: 'EPIC-Garner',
        url: 'https://egarner-staging.epicpartner.vn',
        description: 'Web.Garner',
        createdDate: '08-11-2022',
};

PermissionWebConfig[PermissionWebConst.RealStateWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.RealState,
        name: 'EPIC-Homes',
        url: 'https://egarner-staging.epicpartner.vn',
        description: 'Web.Homes',
        createdDate: '01-03-2023',
};

PermissionWebConfig[PermissionWebConst.LoyaltyWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.Loyalty,
        name: 'EPIC-Loyalty',
        url: 'https://eloyalty-staging.epicpartner.vn',
        description: 'Web.Loyalty',
        createdDate: '19-07-2023',
};

PermissionWebConfig[PermissionWebConst.EventWebsite] = {
        image: '',
        code: '',
        type: PermissionTypes.Web,
        webKey: WebKeys.Event,
        name: 'EPIC-Events',
        url: 'https://events-staging.epicpartner.vn',
        description: 'Web.Events',
        createdDate: '30-07-2023',
};

export default PermissionWebConfig;