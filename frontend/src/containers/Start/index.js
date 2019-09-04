import React from "react";
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import Grid from "@material-ui/core/Grid";
import { userActions } from 'redux/actions';
import { connect } from 'react-redux';
import { UserProfile, ScanData } from './components';

const style = {
  gridItem: {
    padding: 10,
  },
  papersheet: {
    height: '100%'
  },
};

class Start extends React.Component {
  render(){
    return (
      <LayoutWrapper>
        <Grid container spacing={3}>
          <UserProfile />
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
          <ScanData />
        </Grid>
      </LayoutWrapper>
    );
  }
};

const mapSateToProps = state => {
  return {
  };
};

const mapDispatchToProps = {
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(Start);