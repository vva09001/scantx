import React from "react";
import LayoutWrapper from "../components/utility/layoutWrapper";
import Papersheet from "../components/utility/papersheet";
import { HalfColumn } from "../components/utility/rowColumn";
import Grid from "@material-ui/core/Grid";

export default () => (
  <LayoutWrapper>
    <Grid container spacing={3}>
      <Grid item xs={6}>
        <Papersheet title="User:">
          <p>Christian Gathmann</p>
          <p>Mail: gathmann@csbg.de</p>
          <p>Company: CSBG</p>
          <p>Authorization: admin/input/reader</p>
        </Papersheet>
      </Grid>
      <Grid item xs={6}>
        <Papersheet title="Connect QR">
          <p>Scan this QR code to connect your ScanTX app with your computer</p>
          <img
            width="40%"
            src="https://vi.qr-code-generator.com/wp-content/themes/qr/new_structure/markets/basic_market/generator/dist/generator/assets/images/websiteQRCode_noFrame.png"
          ></img>
        </Papersheet>
      </Grid>
      <Grid item xs={6}>
        <Papersheet title="Scan Input: 123">
          <p>Lorem Ipsum...</p>
          <p>Lorem Ipsum...</p>
          <p>Lorem Ipsum...</p>
          <p>...</p>
        </Papersheet>
      </Grid>
    </Grid>
  </LayoutWrapper>
);
