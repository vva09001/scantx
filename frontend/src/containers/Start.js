import React from "react";
import LayoutWrapper from "../components/utility/layoutWrapper";
import Papersheet from "../components/utility/papersheet";
import Grid from "@material-ui/core/Grid";

const style = {
  gridItem: {
    padding: 10,
  },
  papersheet: {
    height: '100%'
  },
};

export default () => {
  return (
    <LayoutWrapper>
      <Grid container spacing={3}>
        <Grid item xs={6} style={style.gridItem}>
          <Papersheet title="User" style={style.papersheet}>
            <p>Christian Gathmann</p>
            <p>Mail: gathmann@csbg.de</p>
            <p>Company: CSBG</p>
            <p>Authorization: admin/input/reader</p>
          </Papersheet>
        </Grid>
        <Grid item xs={6} style={style.gridItem}>
          <Papersheet title="Connect QR" style={style.papersheet}>
            <Grid container direction="row">
            <Grid item xs={6}>
              <p>
                Scan this QR code to connect your ScanTX app with your computer
              </p>
            </Grid>
            <Grid item xs={6}>
              <img
                width="100%"
                src="https://vi.qr-code-generator.com/wp-content/themes/qr/new_structure/markets/basic_market/generator/dist/generator/assets/images/websiteQRCode_noFrame.png"
              ></img>
            </Grid>
            </Grid>
          </Papersheet>
        </Grid>
        <Grid item xs={6} style={style.gridItem}>
          <Papersheet title="Scan Input: 123" style={style.papersheet}>
            <p>Lorem Ipsum...</p>
            <p>Lorem Ipsum...</p>
            <p>Lorem Ipsum...</p>
            <p>...</p>
          </Papersheet>
        </Grid>
      </Grid>
    </LayoutWrapper>
  );
};
