import React, { Component } from "react";
import { connect } from 'react-redux';
import LayoutWrapper from "components/utility/layoutWrapper";
import Button from "components/uielements/button/index.js";
import { CircularProgress } from 'components/uielements/progress';
import { userActions } from 'redux/actions';
import "../styles/style.css";
import {
  FormControl, 
  FormLabel, 
  FormControlLabel, 
  InputLabel, 
  Select, 
  Checkbox,
  Grid,
  TextField
} from "@material-ui/core";

class AddUser extends Component {
  constructor(props) {
    super(props);
    this.state = {
      parasm: {},
      loading: false
    }
  }

  onClick = () => {
    this.setState({
      loading: true
    }, () => {
      this.props.addUser({}, this.onSuccess, this.onSuccess);
    })
  }

  onSuccess = () => {
    this.setState({
      loading: false
    })
  }

  render() {
    return (
      <LayoutWrapper>
        <Grid container justify="center">
          <Grid item xs={8}>
            
          </Grid>
        </Grid>
      </LayoutWrapper>
    );
  }
}

const mapSateToProps = state => {
  return {
  };
};

const mapDispatchToProps = {
  addUser: userActions.addUser
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(AddUser);