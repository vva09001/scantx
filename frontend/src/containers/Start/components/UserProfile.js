import React from "react";
import Grid from "@material-ui/core/Grid";
import Papersheet from "components/utility/papersheet";
import { CircularProgress } from "components/uielements/progress";
import { connect } from "react-redux";

const style = {
  gridItem: {
    padding: 10
  },
  papersheet: {
    height: "100%"
  }
};

class UserProfile extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false
    };
  }

  componentDidMount() {}

  onSuccess = () => {
    this.setState({ loading: false });
  };

  render() {
    const { profile } = this.props;
    return (
      <Grid item xs={6} style={style.gridItem}>
        {this.state.loading ? (
          <CircularProgress />
        ) : (
          <Papersheet title="User" style={style.papersheet}>
            <p>
              Username: <strong>{profile.username}</strong>
            </p>
            <p>
              Mail: <strong>{profile.mail}</strong>
            </p>
            <p>
              Company: <strong>{profile.nameCompany}</strong>
            </p>
            <p>
              Type of Account: <strong>{profile.typeOfAccount}</strong>
            </p>
          </Papersheet>
        )}
      </Grid>
    );
  }
}

const mapSateToProps = state => {
  return {
    profile: state.Auth.profile
  };
};

const mapDispatchToProps = {};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(UserProfile);
