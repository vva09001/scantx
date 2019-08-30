import React, { Component } from "react";
import { connect } from 'react-redux';
import { CircularProgress } from 'components/uielements/progress';
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import { FullColumn } from "components/utility/rowColumn";
import Button from "components/uielements/button/index.js";
import Grid from "components/uielements/grid";
import Table from "components/uielements/table";
import Checkbox from 'components/uielements/checkbox';
import {
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from 'components/uielements/table';
import { companyActions } from 'redux/actions';
import _ from 'lodash';
import "styles/style.css";

class Company extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: true
        }
    }
    componentDidMount() {
        this.props.getCompany(this.onSuccess, this.onSuccess)
    }
    onSuccess = () => {
        this.setState({
            loading: false
        })
    }
    renderData = () => {
        return _.map(this.props.companies, item => (
            <TableRow>
                <TableCell padding="checkbox"><Checkbox /></TableCell>
                <TableCell>{item.name}</TableCell>
                <TableCell>{item.address}</TableCell>
                <TableCell>{item.remark}</TableCell>
            </TableRow>
        ))
    }
    render() {
        if (this.state.loading) {
            return <CircularProgress />;
        }
        return (
            <LayoutWrapper>
                <FullColumn>
                    <Papersheet title="Users of company CSBG">
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell padding="checkbox"><Checkbox /></TableCell>
                                    <TableCell>Name</TableCell>
                                    <TableCell>Address</TableCell>
                                    <TableCell>Remark</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {this.renderData()}
                            </TableBody>
                        </Table>
                        <Grid
                            container
                            direction="row"
                            justify="center"
                            alignItems="center"
                        >
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                                onClick={this.handleAddUser}
                            >
                                Add new user
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                            >
                                Edit selected
                            </Button>
                            <Button
                                className="buttonStyles"
                                variant="contained"
                                color="primary"
                            >
                                Delete selected
                            </Button>
                        </Grid>
                    </Papersheet>
                </FullColumn>
            </LayoutWrapper>
        );
    }
}
const mapSateToProps = state => {
    return {
        companies: state.Company.list
    };
};

const mapDispatchToProps = {
    getCompany: companyActions.getCompany
};
export default connect(
    mapSateToProps,
    mapDispatchToProps
)(Company);